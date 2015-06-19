using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Controls;

namespace HTree_Fractal_Generation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Disables the ability to click incase of double-click.
            DisableMePerClick.IsEnabled = false;

            //Sets the size of Can [Canvas]
            Can.Height = this.ActualHeight;
            Can.Width = this.ActualWidth;

            #region Variable Declarations
            Point StartingPoint = new Point(0, 0);                          //The First Origin Point.
            Random rnd = new Random();
            Point PointB = new Point();                                     //New Point to be used with Origin Point.
            Point PointC = new Point();                                     //New Point to be used with Origin Point.
            int NumberOfLevels = 11;                                        //Number of times the fractal iterates.  Add additional +1 to make it work with FOR loop.
            double lengthOfPointDistance = .75;                             //Distance between each OriginPoint to NewPoint per level.
            double LineThickness = .007;                                    //Thickness used to display the color & and the line itself.
            bool IsXAxis = new bool();                                      //To be used with Main Switch.

            //Minimum & Maximum values of X & Y coordinates.
            double minimumXCoord = 0;
            double minimumYCoord = 0;
            double maximumXCoord = 0;
            double maximumYCoord = 0;
            
            List<Point> printablePoints = new List<Point>();                //Used to print the Points of the H-Tree.
            List<Path> printableLines = new List<Path>();                   //Used to print the Lines of the H-Tree.

            List<Point> originPointList = new List<Point>();                //Will not contain all points to print, only points of intrest. 
            List<Point> temporaryPoints = new List<Point>();                //Temporarily stores points to go into originPointList.
            #endregion

            //Adds first point into the origin list.
            originPointList.Add(StartingPoint);
            

            /* H-Tree Calculations */

            //Will continue until we have reached the depth that was wanted.
            for (int i = 0; i < NumberOfLevels; i++)
            {
                //Swiches the Axis depending on level.  Even levels get Y-Axis, while odd gets X-Axis.
                IsXAxis = !IsXAxis;

                /* [ First Iteration Example ]
                :: Take PointA on X-Axis.
                :: Go 1-X Coord away from PointA in both directions on X-Axis.
                :: Place PointB & PointC on each end of the extended PointA, which is now LineA.  
                :: Repeat for NumberOfLevels.
                */

                //Runs loop until number of origin points equals zero
                while (originPointList.Count != 0)
                {
                    //Creates new points on either X-Axis or Y-Axis.
                    switch (IsXAxis)
                    {
                        case true:

                            //X-Axis
                            PointB = new Point(originPointList[0].X + lengthOfPointDistance,
                                               originPointList[0].Y);

                            PointC = new Point(originPointList[0].X - lengthOfPointDistance,
                                               originPointList[0].Y);

                            //Checks for new Minimum & Maximum X-Coords
                            if (minimumXCoord > PointB.X)
                            { minimumXCoord = PointB.X; }

                            if (maximumXCoord < PointC.X)
                            { maximumXCoord = PointC.X; }

                            if (minimumXCoord > PointC.X)
                            { minimumXCoord = PointC.X; }

                            if (maximumXCoord < PointB.X)
                            { maximumXCoord = PointB.X; }

                            break;

                        case false:

                            //Y-Axis
                            PointB = new Point(originPointList[0].X,
                                               originPointList[0].Y + lengthOfPointDistance);

                            PointC = new Point(originPointList[0].X,
                                               originPointList[0].Y - lengthOfPointDistance);

                            //Checks for new Minimum & Maximum Y-Coords
                            if (minimumYCoord > PointB.Y)
                            { minimumYCoord = PointB.Y; }

                            if (maximumYCoord < PointC.Y)
                            { maximumYCoord = PointC.Y; }

                            if (minimumYCoord > PointC.Y)
                            { minimumYCoord = PointC.Y; }

                            if (maximumYCoord < PointB.Y)
                            { maximumYCoord = PointB.Y; }
                            break;
                    }

                    //Adds both newly created poins into temoraryPoints until moved to originPoints.
                    temporaryPoints.Add(PointB);
                    temporaryPoints.Add(PointC);

                    //Creation of Path from PointB & PointC, or LineA
                    LineGeometry LineA = new LineGeometry(PointB, PointC);

                    //Path to be put onto the Canvas Object(Window).
                    Path LineApth = new Path();

                    //Sets pth's Data & etc..
                    LineApth.Data = LineA;  
                    LineApth.Stroke = Brushes.White;
                    LineApth.StrokeThickness = LineThickness;

                    //Takes current origin point and adds it into the printable list(s).
                    printablePoints.Add(originPointList[0]);

                    //Then removes from origin list.
                    originPointList.Remove(originPointList[0]);

                    //Adds LineApth to printableLines without waiting.
                    printableLines.Add(LineApth);
                }

                //Adds the new origin points to originPointList, their proper home.
                foreach (Point pt in temporaryPoints)
                {
                    originPointList.Add(pt);
                    //temporaryPoints.Remove(pt);
                }

                //Clean up list
                temporaryPoints = new List<Point>();

                //Chop in half the distance between points per level.
                lengthOfPointDistance = lengthOfPointDistance / 1.5;
            }

            //Setting Can's Width and Height.
            Can.Width = maximumYCoord - minimumYCoord;
            Can.Height = maximumXCoord - minimumXCoord;

            //Adds all lines to ZeroCan, to be displayed.
            foreach (Path pth in printableLines)
            {
                ZeroCan.Children.Add(pth);
            }

            double CanHeight = Can.Height / 2;
            double CanWidth = Can.Width / 2;

            //Postion ZeroCan to middle of Can.
            Canvas.SetTop(ZeroCan, CanHeight);
            Canvas.SetLeft(ZeroCan, CanWidth);

            //Re-Enables the ability to clicking.
            DisableMePerClick.IsEnabled = true;
        }
    }
}
