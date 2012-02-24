using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Timers;
using System.Drawing;
using Microsoft.Research.Kinect.Nui;
using Coding4Fun.Kinect.Wpf;
using ShoopDoup;

namespace SkeletalTracking
{
    class ShoopDoupController : SkeletonController
    {
        MainWindow win;
        public ShoopDoupController(MainWindow win)
            : base(win)
        {
            this.win = win;
            this.line = new System.Windows.Shapes.Line();
            line.StrokeThickness = 5;
            line.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            Canvas.SetZIndex(line, -1);
            this.win.MainCanvas.Children.Add(line);
            imageManager = ImageManager.sharedInstance();
            activeImages = new List<ImageTarget>();

            initImageTargets();
            

            rightHandTimer = null;
            rightHandTarget = null;
            rightHandTargetID = -1;
        }


        double highlightedHandBaseDepth;
        double depthDeltaForSelection = .3;
        ImageTarget selectedTarget;
        ImageManager imageManager;
        List<ImageTarget> activeImages;
        ImageTarget primaryImage;
        System.Windows.Shapes.Line line;

        public void resetTargets()
        {
            initImageTargets();
        }

        private void initImageTargets()
        {
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                ImageTarget target = imageManager.getTargetAtIndex(rnd.Next(imageManager.numImages()));
                double angle = -Math.PI / 4.0 * i;

                target.setPosition((int)(290 + Math.Cos(angle) * 200), (int)(290 + Math.Sin(angle) * 200));
                activeImages.Add(target);
                this.win.MainCanvas.Children.Add(target.getGrid());
            }

            primaryImage = imageManager.getTargetAtIndex(rnd.Next(imageManager.numImages()));
            primaryImage.setPosition(290, 290);
            this.win.MainCanvas.Children.Add(primaryImage.getGrid());
        }

        public override void processSkeletonFrame(SkeletonData skeleton, Dictionary<int, Target> targets)
        {
            Joint chest = skeleton.Joints[JointID.ShoulderCenter].ScaleTo(640, 480, window.k_xMaxJointScale, window.k_yMaxJointScale);
            highlightedHandBaseDepth = chest.Position.Z;

            Joint rightHand = skeleton.Joints[JointID.HandRight].ScaleTo(640, 480, window.k_xMaxJointScale, window.k_yMaxJointScale);

            foreach (var cur in activeImages)
            {

                //Calculate how far our right hand is from the target in both x and y directions
                double deltaX_right = Math.Abs(rightHand.Position.X - cur.getX());
                double deltaY_right = Math.Abs(rightHand.Position.Y - cur.getY());

                //If we have a hit in a reasonable range, highlight the target
                if (deltaX_right < 60 && deltaY_right < 60 && !cur.isPermanentlySelected())
                {
                    Console.WriteLine("Right hand: " + rightHand.Position.Z + " \t Chest: " + highlightedHandBaseDepth);
                    if (Math.Abs(rightHand.Position.Z - highlightedHandBaseDepth) > depthDeltaForSelection)
                    {
                        if (cur.isHighlighted() || cur.isSelected())
                        {
                            cur.setSelected(true);

                            selectedTarget = cur;
                        }
                        else
                        {
                            cur.setSelected(true);
                        }
                    }
                    else
                    {
                        cur.setHighlighted(true);
                    }
                }
                else
                {
                    cur.setSelected(false);
                    cur.setHighlighted(false);
                }
            }

            if (selectedTarget != null && (Math.Abs(rightHand.Position.Z - highlightedHandBaseDepth) > depthDeltaForSelection) && !selectedTarget.isPermanentlySelected())
            {
               
                double deltaX_right = Math.Abs(rightHand.Position.X - primaryImage.getX());
                double deltaY_right = Math.Abs(rightHand.Position.Y - primaryImage.getY());

                //If we have a hit in a reasonable range, highlight the target
                if (deltaX_right < 60 && deltaY_right < 60)
                {
                    primaryImage.setHighlighted(true);
                    selectedTarget.setPermanentlySelected();

                    line.X1 = selectedTarget.getX();
                    line.Y1 = selectedTarget.getY();
                    line.X2 = primaryImage.getX();
                    line.Y2 = primaryImage.getY();

                    line = new System.Windows.Shapes.Line();
                    line.StrokeThickness = 5;
                    line.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    Canvas.SetZIndex(line, -1);
                  
                    win.MainCanvas.Children.Add(line);

                    selectedTarget = null;
                }
                else
                {
                    primaryImage.setHighlighted(false);
                    line.Visibility = System.Windows.Visibility.Visible;
                    line.X1 = selectedTarget.getX();
                    line.X2 = rightHand.Position.X;
                    line.Y1 = selectedTarget.getY();
                    line.Y2 = rightHand.Position.Y;
                }
            }
            else
            {
                line.Visibility = System.Windows.Visibility.Hidden;
                selectedTarget = null;
            }

            /* (firstTimeSeeingSkeleton)
            {
                highlightedHandDepth = rightHand.Position.Z;
                firstTimeSeeingSkeleton = false;
            }*/
            /*
            foreach (var target in targets)
            {
                Target curr = target.Value;
                if (!curr.isHidden())// && targetBasePositions.ContainsKey(target.Value.id))
                {
                    double targetSlope = (curr.getYPosition() - rightElbow.Position.Y) / (curr.getXPosition() - rightElbow.Position.X);
                    double elbowSlope = (rightHand.Position.Y - rightElbow.Position.Y) / (rightHand.Position.X - rightElbow.Position.X);

                    if ((targetSlope + .2) > elbowSlope && (targetSlope - .2) < elbowSlope)
                    {
                        curr.setTargetHighlighted();

                        if (highlightedTarget != curr)
                        {
                            Console.WriteLine("Setting highlightedTarget to " + curr.id + ". z depth is " + rightHand.Position.Z);
                            highlightedTarget = curr;
                        }
                    }
                    else
                    {
                        curr.setTargetUnselected();
                    }
                }
                else
                {
                    curr.setTargetUnselected();
                }
            }

            if (highlightedTarget != null && highlightedHandDepth - rightHand.Position.Z > depthDeltaForSelection)
            {
                highlightedTarget.setTargetSelected();
            }
            else if (highlightedTarget != null && highlightedHandDepth - rightHand.Position.Z < depthDeltaForSelection)
            {
                highlightedTarget.setTargetUnselected();
                highlightedTarget.setTargetHighlighted();
            }   */
        }
        
        public override void controllerActivated(Dictionary<int, Target> targets)
        {

        }
    }
}
