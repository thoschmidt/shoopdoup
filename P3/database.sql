-- phpMyAdmin SQL Dump
-- version 3.3.10.4
-- http://www.phpmyadmin.net
--
-- Host: mysql.cs147.org
-- Generation Time: Feb 21, 2012 at 05:41 PM
-- Server version: 5.1.39
-- PHP Version: 5.2.17

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `akonig_mysql`
--

-- --------------------------------------------------------

--
-- Table structure for table `movies`
--

CREATE TABLE IF NOT EXISTS `movies` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(150) NOT NULL,
  `img_link` varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=101 ;

--
-- Dumping data for table `movies`
--

INSERT INTO `movies` (`id`, `name`, `img_link`) VALUES
(1, 'Star Wars Episode V: The Empire Strikes Back', 'http://cf2.imgobject.com/t/p/original/6u1fYtxG5eqjhtCPDx04pJphQRW.jpg'),
(2, 'Star Wars Episode II: Attack of the Clones', 'http://cf2.imgobject.com/t/p/original/2vcNFtrZXNwIcBgH5e2xXCmVR8t.jpg'),
(3, 'J. Edgar', 'http://cf2.imgobject.com/t/p/original/upACO9i21lb5lG7ovf51m3lIPcv.jpg'),
(4, 'Star Wars Episode I: The Phantom Menace', 'http://cf2.imgobject.com/t/p/original/n8V09dDc02KsSN6Q4hC2BX6hN8X.jpg'),
(5, 'The Shawshank Redemption', 'http://cf2.imgobject.com/t/p/original/XphTuZGnDcxZRJwVc0a27cKpNi.jpg'),
(6, 'The Godfather', 'http://cf2.imgobject.com/t/p/original/d4KNaTrltq6bpkFS01pYtyXa09m.jpg'),
(7, 'The Descendants', 'http://cf2.imgobject.com/t/p/original/3nnKjnTu347oHrZttoBtK5G2KSJ.jpg'),
(8, 'Schindler''s List', 'http://cf2.imgobject.com/t/p/original/tvOvW7Qjj63zbQW5TZ8CjPThAUd.jpg'),
(9, 'Casablanca', 'http://cf2.imgobject.com/t/p/original/sm1QVZu5RKe1vXVHZooo4SZyHMx.jpg'),
(10, 'The Lion King', 'http://cf2.imgobject.com/t/p/original/bKPtXn9n4M4s8vvZrbw40mYsefB.jpg'),
(11, 'Toy Story', 'http://cf2.imgobject.com/t/p/original/d0Bb7MMhxyDDrxnvcLIk3R2uWDU.jpg'),
(12, 'The Bourne Identity', 'http://cf2.imgobject.com/t/p/original/bXQIL36VQdzJ69lcjQR1WQzJqQR.jpg'),
(13, 'Back to the Future', 'http://cf2.imgobject.com/t/p/original/pTpxQB1N0waaSc3OSn0e9oc8kx9.jpg'),
(14, 'Citizen Kane', 'http://cf2.imgobject.com/t/p/original/92ysP7ACHiNmD6H6W2GjZsexIvw.jpg'),
(15, 'The Dark Knight', 'http://cf2.imgobject.com/t/p/original/zgUJiOCEKnvpkcjZxMwCPQV4QCU.jpg'),
(16, 'Fight Club', 'http://cf2.imgobject.com/t/p/original/jQ2iUsXI2jmUcOFGjOaONCLwaVp.jpg'),
(17, 'Memento', 'http://cf2.imgobject.com/t/p/original/5eqZQpp0gFhk5zDuWdKclnGVs5T.jpg'),
(18, 'Toy Story 3', 'http://cf2.imgobject.com/t/p/original/ocdzo5jXxYngxhQM38vzNr3Ezco.jpg'),
(19, 'V for Vendetta', 'http://cf2.imgobject.com/t/p/original/wlYPNBApf49XK30M0ML8YfnNHen.jpg'),
(20, 'Spirited Away', 'http://cf2.imgobject.com/t/p/original/1pbN4p3uqCBCySyy2nEYsUVYSTM.jpg'),
(21, 'The Matrix', 'http://cf2.imgobject.com/t/p/original/mAC9D2BmhtkVjcClDWrMLJteHYv.jpg'),
(22, 'Pulp Fiction', 'http://cf2.imgobject.com/t/p/original/dnBQzLDcCbAzhkgo37q4vnWlPZU.jpg'),
(23, 'Forrest Gump', 'http://cf2.imgobject.com/t/p/original/iZvCkb34CAmV9BETIrHY4yiS115.jpg'),
(24, 'Jaws', 'http://cf2.imgobject.com/t/p/original/l1yltvzILaZcx2jYvc5sEMkM7Eh.jpg'),
(25, 'Saving Private Ryan', 'http://cf2.imgobject.com/t/p/original/9UwfRlvq6Eekewf3QAW5Plx0iEL.jpg'),
(26, 'City of God', 'http://cf2.imgobject.com/t/p/original/gCqnQaq8T4CfioP9uETLx9iMJF4.jpg'),
(27, 'The Lord of the Rings: The Return of the King', 'http://cf2.imgobject.com/t/p/original/j6NCjU6Zh7SkfIeN5zDaoTmBn4m.jpg'),
(28, 'Kung Fu Hustle', 'http://cf2.imgobject.com/t/p/original/yeo7GsADzwK39V9SgiRAIClMFGp.jpg'),
(29, 'Star Wars Episode VI: Return of the Jedi', 'http://cf2.imgobject.com/t/p/original/jx5p0aHlbPXqe3AH9G15NvmWaqQ.jpg'),
(30, 'Braveheart', 'http://cf2.imgobject.com/t/p/original/fn0GT3o5WLJfeuJhOtC9XR7PYtv.jpg'),
(31, 'The Lord of the Rings: The Fellowship of the Ring', 'http://cf2.imgobject.com/t/p/original/9HG6pINW1KoFTAKY3LdybkoOKAm.jpg'),
(32, 'Downfall', 'http://cf2.imgobject.com/t/p/original/jOs0I1g4bzUnUhDz4elREczbMQL.jpg'),
(33, 'Home Alone', 'http://cf2.imgobject.com/t/p/original/1X3gc49cOtNQ57L6GGAb9cb9HJ8.jpg'),
(34, 'Inception', 'http://cf2.imgobject.com/t/p/original/ziKvu3Th9l1wN2aIeVj5ElpBqFu.jpg'),
(35, 'The Incredibles', 'http://cf2.imgobject.com/t/p/original/jjAgMfj0TAPvdC8E5AqDm2BBeYz.jpg'),
(36, 'Meet the Robinsons', 'http://cf2.imgobject.com/t/p/original/oe7w9SxGmpSW1dlz5QaVVKWBEVz.jpg'),
(37, 'Rush Hour', 'http://cf2.imgobject.com/t/p/original/jdfxpW5LF36sHsHjyH8CMBEG4TF.jpg'),
(38, 'Finding Nemo', 'http://cf2.imgobject.com/t/p/original/tQv3nfFHMe4PQzPXaQzQFftPPTA.jpg'),
(39, 'Monsters Inc.', 'http://cf2.imgobject.com/t/p/original/jDejwTPLzmkAXPwCbjnZiBekVnf.jpg'),
(40, 'Die Hard', 'http://cf2.imgobject.com/t/p/original/aUvvB8Z5IcpJPw9ZZBv7wtL2QLj.jpg'),
(41, 'Spider-Man', 'http://cf2.imgobject.com/t/p/original/iGncxfVLgXzFt1owUDeA5NmSG7f.jpg'),
(42, 'Spider-Man 2', 'http://cf2.imgobject.com/t/p/original/qtBFrsEQ4oXW8sKvRxkKnYuPLg.jpg'),
(43, 'Spider-Man 3', 'http://cf2.imgobject.com/t/p/original/uC2pAMjb32NIgQ1GdC1Bl6LZJc2.jpg'),
(44, 'Green Lantern', 'http://cf2.imgobject.com/t/p/original/sVw0RlPnWca8w7Zb8CaxOdeHESb.jpg'),
(45, 'How to Train Your Dragon', 'http://cf2.imgobject.com/t/p/original/zMAm3WYmvD40FaWFsOmpicQFabz.jpg'),
(46, 'The Exorcist', 'http://cf2.imgobject.com/t/p/original/7M0UmuvzojEAlyAtBnfvgjrDLtt.jpg'),
(47, 'Zoolander', 'http://cf2.imgobject.com/t/p/original/3XDIoixx9ZHasJeu9cWE2ORSpnh.jpg'),
(48, 'Titanic', 'http://cf2.imgobject.com/t/p/original/irIzAX088ebnvKi8XBccZnLGOso.jpg'),
(49, 'Tropic Thunder', 'http://cf2.imgobject.com/t/p/original/zoWUdaaWKPDyr9b0il0YcggDWgJ.jpg'),
(50, 'The Parent Trap', 'http://cf2.imgobject.com/t/p/original/pETgdAWlCvmZmTpJH9Nj2lrYpU4.jpg'),
(51, 'War Horse', 'http://cf2.imgobject.com/t/p/original/sMf4OpSjOyMLosDbcyZq68WR0U1.jpg'),
(52, 'The King''s Speech', 'http://cf2.imgobject.com/t/p/original/qfzja8SyObEKISymZBbH9QisS9g.jpg'),
(53, 'Troy', 'http://cf2.imgobject.com/t/p/original/z7iMeZpNAxUIzD2oJfvoaq8YkT8.jpg'),
(54, 'The Blind Side', 'http://cf2.imgobject.com/t/p/original/i0CXhh80ALgtzW6kvc9SKz9QlNP.jpg'),
(55, 'Moneyball', 'http://cf2.imgobject.com/t/p/original/3oAa8mJJ97CH9AeGEY6vjAxqcvZ.jpg'),
(56, 'The Proposal', 'http://cf2.imgobject.com/t/p/original/xippClkb4VCE15uva13SAGf8Vsd.jpg'),
(57, 'Avatar', 'http://cf2.imgobject.com/t/p/original/tcqb9NHdw9SWs2a88KCDD4V8sVR.jpg'),
(58, 'Crazy, Stupid, Love', 'http://cf2.imgobject.com/t/p/original/vt3tfVYuDdXS8l1dC6Uwe2dxuUb.jpg'),
(59, 'Kingdom of Heaven', 'http://cf2.imgobject.com/t/p/original/2pE4pTT8IXF56fiWMOk7cv7Fkpi.jpg'),
(60, 'Shrek', 'http://cf2.imgobject.com/t/p/original/4ybHCYMyUUaO2xjB80zh4KjkKsl.jpg'),
(61, 'Shrek 2', 'http://cf2.imgobject.com/t/p/original/rD8SvOTCCJ2VIpIV7GUwUKD1Kzc.jpg'),
(62, 'Shrek the Third', 'http://cf2.imgobject.com/t/p/original/lFY8bycWxdcQdAU2FHYqbbxVFiZ.jpg'),
(63, 'Jurassic Park', 'http://cf2.imgobject.com/t/p/original/utO5haqglKdoDH9qKcgGQwCq0lS.jpg'),
(64, 'Harry Potter and the Sorcerer''s Stone', 'http://cf2.imgobject.com/t/p/original/uLGaJ9FgPWf7EUgwjp9RTmHemw8.jpg'),
(65, 'Harry Potter and the Order of the Phoenix', 'http://cf2.imgobject.com/t/p/original/lmYmoXVzVrTadfXHZ3v1ySqA1fn.jpg'),
(66, 'Independence Day', 'http://cf2.imgobject.com/t/p/original/nkj3HRG3g2zjlKRWmjJHvElhX4y.jpg'),
(67, 'The Da Vinci Code', 'http://cf2.imgobject.com/t/p/original/6P3UTlxSk3Y7GWWF5Y0ek1YJkH7.jpg'),
(68, 'Ocean''s Eleven', 'http://cf2.imgobject.com/t/p/original/o0h76DVXvk5OKjmNez5YY0GODC2.jpg'),
(69, 'Ocean''s Twelve', 'http://cf2.imgobject.com/t/p/original/q71ovWCbeWGRnGmZCLaPLWQLQA.jpg'),
(70, 'Ocean''s Thirteen', 'http://cf2.imgobject.com/t/p/original/mK8COrnt8IZah0pEPjuRKpHSSgF.jpg'),
(71, 'Casino Royale', 'http://cf2.imgobject.com/t/p/original/uOlK5rqGzspNeHmamFGPjPxUq2a.jpg'),
(72, 'Quantum of Solace', 'http://cf2.imgobject.com/t/p/original/mQRtQoCAjCCeO68MGMz0dR7Ftzv.jpg'),
(73, 'Die Another Day', 'http://cf2.imgobject.com/t/p/original/7ph8lZ3EXe4okGWkYYPJTbC3mrQ.jpg'),
(74, 'Into the Wild', 'http://cf2.imgobject.com/t/p/original/lHyYgaocXR6KcJLxVmxZDj115hH.jpg'),
(75, 'Garden State', 'http://cf2.imgobject.com/t/p/original/hdjPvEjnvDN5Y1yoMkUhk4QBIUl.jpg'),
(76, 'Ice Age', 'http://cf2.imgobject.com/t/p/original/7vYhg6vQnLlNC2zMUUO8CZG3rd8.jpg'),
(77, 'G-Force', 'http://cf2.imgobject.com/t/p/original/ecTQ40F6Ob4JW2qMRUf8ThxViyd.jpg'),
(78, 'Ghost Rider', 'http://cf2.imgobject.com/t/p/original/vStrjv8HjyrvxD8Unpre89oMYpj.jpg'),
(79, 'National Treasure', 'http://cf2.imgobject.com/t/p/original/luMoc56LLMWUt60vUNNpwxrbTNt.jpg'),
(80, 'National Treasure: Book of Secrets', 'http://cf2.imgobject.com/t/p/original/5fOwo57lLZ3TFPG5jL6Db9Qaq8Q.jpg'),
(81, 'Alice in Wonderland', 'http://cf2.imgobject.com/t/p/original/gDHVodPe6zLjIO02gdczQgLxD8Q.jpg'),
(82, 'The Curious Case of Benjamin Button', 'http://cf2.imgobject.com/t/p/original/4O4INOPtWTfHq3dd5vYTPV0TCwa.jpg'),
(83, 'House of Wax', 'http://cf2.imgobject.com/t/p/original/mlLHueKU7upuFHowQK80O96xp7O.jpg'),
(84, 'Seabiscuit', 'http://cf2.imgobject.com/t/p/original/hgc8hb5G38nphRmHJv6BRJ0oLcD.jpg'),
(85, 'American Pie', 'http://cf2.imgobject.com/t/p/original/7OAgvsZj6LA15KCXxrtVtXkFZhr.jpg'),
(86, 'Up in the Air', 'http://cf2.imgobject.com/t/p/original/ajwiDmJbXlEIKJBcrXy8RN6M3G5.jpg'),
(87, 'Up', 'http://cf2.imgobject.com/t/p/original/zh9DXJhBdHVVaWiDURTipADamcK.jpg'),
(88, 'The Chronicles of Narnia: The Lion, The Witch and the Wardrobe', 'http://cf2.imgobject.com/t/p/original/l1NNAvgIj5QVpbJNp8GN7KCyl3f.jpg'),
(89, 'The Count of Monte Cristo', 'http://cf2.imgobject.com/t/p/original/u1tLUDAMjOutrnSCyDhiiyZs50L.jpg'),
(90, 'The American President', 'http://cf2.imgobject.com/t/p/original/lymPNGLZgPHuqM29rKMGV46ANij.jpg'),
(91, 'The Matrix Reloaded', 'http://cf2.imgobject.com/t/p/original/ezIurBz2fdUc68d98Fp9dRf5ihv.jpg'),
(92, 'Animal House', 'http://cf2.imgobject.com/t/p/original/9rOksA6g8WvXFJz5VKVFyt3spqC.jpg'),
(93, 'The Other Guys', 'http://cf2.imgobject.com/t/p/original/b4mYjC8kKx32BnCNBQeDCA76WBd.jpg'),
(94, 'Mission: Impossible', 'http://cf2.imgobject.com/t/p/original/eYl3PmkOGvRj4ETS4udyblO3BfS.jpg'),
(95, 'Mission: Impossible II', 'http://cf2.imgobject.com/t/p/original/6e0zLGUB9NX1QLkeMPFZgPPBiMb.jpg'),
(96, 'Mission: Impossible III', 'http://cf2.imgobject.com/t/p/original/usXoT2TCWky5hlipOFQ64qB2KiN.jpg'),
(97, 'Men in Black', 'http://cf2.imgobject.com/t/p/original/f24UVKq3UiQWLqGWdqjwkzgB8j8.jpg'),
(98, 'Batman Begins', 'http://cf2.imgobject.com/t/p/original/8kufTLzrHb2QzzowExtxp1EDmF5.jpg'),
(99, 'Superman Returns', 'http://cf2.imgobject.com/t/p/original/vEufsHYtmE362NOWuf1hAlvqrPQ.jpg'),
(100, 'Wall Street', 'http://cf2.imgobject.com/t/p/original/jjxkZApCeCwHTnI0kk85fVHVslC.jpg');

-- --------------------------------------------------------

--
-- Table structure for table `relations`
--

CREATE TABLE IF NOT EXISTS `relations` (
  `from_id` int(20) NOT NULL,
  `to_id` int(20) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `relations`
--

INSERT INTO `relations` (`from_id`, `to_id`) VALUES
(1, 2),
(2, 1);
