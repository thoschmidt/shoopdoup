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
            this.win.MainCanvas.Children.Add(line);

            this.newImageTarget = new ImageTarget("poop");
            newImageTarget.setPosition(100, 100);
            this.win.MainCanvas.Children.Add(newImageTarget.getGrid());
            

            rightHandTimer = null;
            rightHandTarget = null;
            rightHandTargetID = -1;
        }


        double highlightedHandBaseDepth;
        double depthDeltaForSelection = .3;
        Target selectedTarget;
        ImageTarget newImageTarget;
        System.Windows.Shapes.Line line;

        public override void processSkeletonFrame(SkeletonData skeleton, Dictionary<int, Target> targets)
        {
            Joint chest = skeleton.Joints[JointID.ShoulderCenter].ScaleTo(640, 480, window.k_xMaxJointScale, window.k_yMaxJointScale);
            highlightedHandBaseDepth = chest.Position.Z;

            Joint rightHand = skeleton.Joints[JointID.HandRight].ScaleTo(640, 480, window.k_xMaxJointScale, window.k_yMaxJointScale);

            foreach (var target in targets)
            {
                Target cur = target.Value;
                int targetID = cur.id; //ID in range [1..5]

                //Calculate how far our right hand is from the target in both x and y directions
                double deltaX_right = Math.Abs(rightHand.Position.X - cur.getXPosition());
                double deltaY_right = Math.Abs(rightHand.Position.Y - cur.getYPosition());

                //If we have a hit in a reasonable range, highlight the target
                if (deltaX_right < 60 && deltaY_right < 60)
                {
                    Console.WriteLine("Right hand: " + rightHand.Position.Z + " \t Chest: " + highlightedHandBaseDepth);
                    if (Math.Abs(rightHand.Position.Z - highlightedHandBaseDepth) > depthDeltaForSelection)
                    {
                        if (cur.isHighlighted() || cur.isSelected())
                        {
                            cur.setTargetSelected();

                            selectedTarget = cur;
                        }
                        else
                        {
                            cur.setTargetTranslucent();
                        }
                    }
                    else
                    {
                        cur.setTargetHighlighted();
                    }
                }
                else
                {
                    cur.setTargetUnselected();
                }
            }

            if (selectedTarget != null && (Math.Abs(rightHand.Position.Z - highlightedHandBaseDepth) > depthDeltaForSelection))
            {
                line.Visibility = System.Windows.Visibility.Visible;
                line.X1 = selectedTarget.getXPosition();
                line.X2 = rightHand.Position.X;
                line.Y1 = selectedTarget.getYPosition();
                line.Y2 = rightHand.Position.Y;
                Console.WriteLine("Origin: " + line.X1 + " Hand Location: " + line.X2);
                line.StrokeThickness = 5;
                line.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));


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

            /* YOUR CODE HERE */

        }
    }
}
