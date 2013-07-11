﻿namespace GenArt.Classes
{
    public class Settings
    {
        public static int ActiveAddPointMutationRate = 150;
        public static int ActiveAddPolygonMutationRate = 70;
        public static int ActiveMovePolygonMutationRate = 1;
        public static int ActiveRemovePointMutationRate = 150;
        public static int ActiveRemovePolygonMutationRate = 150;
        public static int ActiveAlphaMutationRate = 1500;
        public static int ActiveAlphaRangeMax = 254;
        public static int ActiveAlphaRangeMin = 0;
        public static int ActiveBlueMutationRate = 150;
        public static int ActiveBlueRangeMax = 255;
        public static int ActiveBlueRangeMin;
        public static int ActiveGreenMutationRate = 150;
        public static int ActiveGreenRangeMax = 255;
        public static int ActiveGreenRangeMin;
        public static int ActiveMovePointMaxMutationRate = 1;
        public static int ActiveMovePointMidMutationRate = 1;
        public static int ActiveMovePointMinMutationRate = 1;

        public static int ActiveMovePointRangeMid = 100;
        public static int ActiveMovePointRangeMin = 3;
        
        public static int ActivePointsMax = 150;
        public static int ActivePointsMin;
        public static int ActivePointsPerPolygonMax = 3;
        public static int ActivePointsPerPolygonMin = 3;
        public static int ActivePolygonsMax = 255;
        public static int ActivePolygonsMin;
        public static int ActiveRedMutationRate = 150;
        public static int ActiveRedRangeMax = 255;
        public static int ActiveRedRangeMin;
        
        private int addPointMutationRate = 150;

        //Mutation rates
        private int addPolygonMutationRate = 70;
        private int alphaMutationRate = 1500;
        private int alphaRangeMax = 254;
        private int alphaRangeMin = 0;
        private int blueMutationRate = 1500;
        private int blueRangeMax = 255;
        private int blueRangeMin;
        private int greenMutationRate = 1500;
        private int greenRangeMax = 255;
        private int greenRangeMin;
        private int movePointMaxMutationRate = 1500;
        private int movePointMidMutationRate = 150;
        private int movePointMinMutationRate = 150;
        private int movePointRangeMid = 20;
        private int movePointRangeMin = 3;
        private int movePolygonMutationRate = 1;
        private int pointsMax = 150;
        private int pointsMin;
        private int pointsPerPolygonMax = 10;
        private int pointsPerPolygonMin = 3;
        private int polygonsMax = 255;
        private int polygonsMin;
        private int redMutationRate = 1500;
        private int redRangeMax = 255;
        private int redRangeMin;
        private int removePointMutationRate = 150;

        private int removePolygonMutationRate = 150;

        public Settings()
        {
            Reset();
        }

        public int AddPolygonMutationRate
        {
            get { return addPolygonMutationRate; }
            set
            {
                addPolygonMutationRate = value;
                //this.OnPropertyChanged("AddPolygonMutationRate");
            }
        }

        public int RemovePolygonMutationRate
        {
            get { return removePolygonMutationRate; }
            set { removePolygonMutationRate = value; }
        }

        public int MovePolygonMutationRate
        {
            get { return movePolygonMutationRate; }
            set { movePolygonMutationRate = value; }
        }

        public int AddPointMutationRate
        {
            get { return addPointMutationRate; }
            set { addPointMutationRate = value; }
        }

        public int RemovePointMutationRate
        {
            get { return removePointMutationRate; }
            set { removePointMutationRate = value; }
        }

        public int MovePointMaxMutationRate
        {
            get { return movePointMaxMutationRate; }
            set { movePointMaxMutationRate = value; }
        }

        public int MovePointMidMutationRate
        {
            get { return movePointMidMutationRate; }
            set { movePointMidMutationRate = value; }
        }

        public int MovePointMinMutationRate
        {
            get { return movePointMinMutationRate; }
            set { movePointMinMutationRate = value; }
        }

        public int RedMutationRate
        {
            get { return redMutationRate; }
            set { redMutationRate = value; }
        }

        public int GreenMutationRate
        {
            get { return greenMutationRate; }
            set { greenMutationRate = value; }
        }

        public int BlueMutationRate
        {
            get { return blueMutationRate; }
            set { blueMutationRate = value; }
        }

        public int AlphaMutationRate
        {
            get { return alphaMutationRate; }
            set { alphaMutationRate = value; }
        }

        //Ranges

        public int RedRangeMin
        {
            get { return redRangeMin; }
            set
            {
                if (value > redRangeMax)
                    RedRangeMax = value;

                redRangeMin = value;
            }
        }

        public int RedRangeMax
        {
            get { return redRangeMax; }
            set
            {
                if (value < redRangeMin)
                    RedRangeMin = value;

                redRangeMax = value;
            }
        }

        public int GreenRangeMin
        {
            get { return greenRangeMin; }
            set
            {
                if (value > greenRangeMax)
                    GreenRangeMax = value;

                greenRangeMin = value;
            }
        }

        public int GreenRangeMax
        {
            get { return greenRangeMax; }
            set
            {
                if (value < greenRangeMin)
                    GreenRangeMin = value;

                greenRangeMax = value;
            }
        }

        public int BlueRangeMin
        {
            get { return blueRangeMin; }
            set
            {
                if (value > blueRangeMax)
                    BlueRangeMax = value;

                blueRangeMin = value;
            }
        }

        public int BlueRangeMax
        {
            get { return blueRangeMax; }
            set
            {
                if (value < blueRangeMin)
                    BlueRangeMin = value;

                blueRangeMax = value;
            }
        }

        public int AlphaRangeMin
        {
            get { return alphaRangeMin; }
            set
            {
                if (value > alphaRangeMax)
                    AlphaRangeMax = value;

                alphaRangeMin = value;
            }
        }

        public int AlphaRangeMax
        {
            get { return alphaRangeMax; }
            set
            {
                if (value < alphaRangeMin)
                    AlphaRangeMin = value;

                alphaRangeMax = value;
            }
        }

        public int PolygonsMin
        {
            get { return polygonsMin; }
            set
            {
                if (value > polygonsMax)
                    PolygonsMax = value;

                polygonsMin = value;
            }
        }

        public int PolygonsMax
        {
            get { return polygonsMax; }
            set
            {
                if (value < polygonsMin)
                    PolygonsMin = value;

                polygonsMax = value;
            }
        }

        public int PointsPerPolygonMin
        {
            get { return pointsPerPolygonMin; }
            set
            {
                if (value > pointsPerPolygonMax)
                    PointsPerPolygonMax = value;

                if (value < 3)
                    return;

                pointsPerPolygonMin = value;
            }
        }

        public int PointsPerPolygonMax
        {
            get { return pointsPerPolygonMax; }
            set
            {
                if (value < pointsPerPolygonMin)
                    PointsPerPolygonMin = value;

                pointsPerPolygonMax = value;
            }
        }

        public int PointsMin
        {
            get { return pointsMin; }
            set
            {
                if (value > pointsMax)
                    PointsMax = value;

                pointsMin = value;
            }
        }

        public int PointsMax
        {
            get { return pointsMax; }
            set
            {
                if (value < pointsMin)
                    PointsMin = value;

                pointsMax = value;
            }
        }

        public int MovePointRangeMin
        {
            get { return movePointRangeMin; }
            set
            {
                if (value > movePointRangeMid)
                    MovePointRangeMid = value;

                movePointRangeMin = value;
            }
        }

        public int MovePointRangeMid
        {
            get { return movePointRangeMid; }
            set
            {
                if (value < movePointRangeMin)
                    MovePointRangeMin = value;

                movePointRangeMid = value;
            }
        }

        public void Activate()
        {
            ActiveAddPolygonMutationRate = AddPolygonMutationRate;
            ActiveRemovePolygonMutationRate = RemovePolygonMutationRate;
            ActiveMovePolygonMutationRate = MovePolygonMutationRate;

            ActiveAddPointMutationRate = AddPointMutationRate;
            ActiveRemovePointMutationRate = RemovePointMutationRate;
            ActiveMovePointMaxMutationRate = MovePointMaxMutationRate;
            ActiveMovePointMidMutationRate = MovePointMidMutationRate;
            ActiveMovePointMinMutationRate = MovePointMinMutationRate;

            ActiveRedMutationRate = RedMutationRate;
            ActiveGreenMutationRate = GreenMutationRate;
            ActiveBlueMutationRate = BlueMutationRate;
            ActiveAlphaMutationRate = AlphaMutationRate;

            //Limits / Constraints
            ActiveRedRangeMin = RedRangeMin;
            ActiveRedRangeMax = RedRangeMax;
            ActiveGreenRangeMin = GreenRangeMin;
            ActiveGreenRangeMax = GreenRangeMax;
            ActiveBlueRangeMin = BlueRangeMin;
            ActiveBlueRangeMax = BlueRangeMax;
            ActiveAlphaRangeMin = AlphaRangeMin;
            ActiveAlphaRangeMax = AlphaRangeMax;

            ActivePolygonsMax = PolygonsMax;
            ActivePolygonsMin = PolygonsMin;

            ActivePointsPerPolygonMax = PointsPerPolygonMax;
            ActivePointsPerPolygonMin = PointsPerPolygonMin;

            ActivePointsMax = PointsMax;
            ActivePointsMin = PointsMin;

            ActiveMovePointRangeMid = MovePointRangeMid;
            ActiveMovePointRangeMin = MovePointRangeMin;
        }

        public void Discard()
        {
            AddPolygonMutationRate = ActiveAddPolygonMutationRate;
            RemovePolygonMutationRate = ActiveRemovePolygonMutationRate;
            MovePolygonMutationRate = ActiveMovePolygonMutationRate;

            AddPointMutationRate = ActiveAddPointMutationRate;
            RemovePointMutationRate = ActiveRemovePointMutationRate;
            MovePointMaxMutationRate = ActiveMovePointMaxMutationRate;
            MovePointMidMutationRate = ActiveMovePointMidMutationRate;
            MovePointMinMutationRate = ActiveMovePointMinMutationRate;

            RedMutationRate = ActiveRedMutationRate;
            GreenMutationRate = ActiveGreenMutationRate;
            BlueMutationRate = ActiveBlueMutationRate;
            AlphaMutationRate = ActiveAlphaMutationRate;

            //Limits / Constraints
            RedRangeMin = ActiveRedRangeMin;
            RedRangeMax = ActiveRedRangeMax;
            GreenRangeMin = ActiveGreenRangeMin;
            GreenRangeMax = ActiveGreenRangeMax;
            BlueRangeMin = ActiveBlueRangeMin;
            BlueRangeMax = ActiveBlueRangeMax;
            AlphaRangeMin = ActiveAlphaRangeMin;
            AlphaRangeMax = ActiveAlphaRangeMax;

            PolygonsMax = ActivePolygonsMax;
            PolygonsMin = ActivePolygonsMin;

            PointsPerPolygonMax = ActivePointsPerPolygonMax;
            PointsPerPolygonMin = ActivePointsPerPolygonMin;

            PointsMax = ActivePointsMax;
            PointsMin = ActivePointsMin;

            MovePointRangeMid = ActiveMovePointRangeMid;
            MovePointRangeMin = ActiveMovePointRangeMin;
        }

        public void Reset()
        {
            ActiveAddPolygonMutationRate = 16;
            ActiveRemovePolygonMutationRate = 16;
            ActiveMovePolygonMutationRate = 8;

            ActiveAddPointMutationRate = 200;
            ActiveRemovePointMutationRate = 100;
            ActiveMovePointMaxMutationRate = 300;
            ActiveMovePointMidMutationRate = 200;
            ActiveMovePointMinMutationRate = 50;

            ActiveRedMutationRate = 100;
            ActiveGreenMutationRate = 100;
            ActiveBlueMutationRate = 100;
            ActiveAlphaMutationRate = 100;

            //Limits / Constraints
            ActiveRedRangeMin = 0;
            ActiveRedRangeMax = 255;
            ActiveGreenRangeMin = 0;
            ActiveGreenRangeMax = 255;
            ActiveBlueRangeMin = 0;
            ActiveBlueRangeMax = 255;
            ActiveAlphaRangeMin = 10;
            ActiveAlphaRangeMax = 254;

            ActivePolygonsMax = 255;
            ActivePolygonsMin = 0;

            ActivePointsPerPolygonMax = 3;
            ActivePointsPerPolygonMin = 3;

            ActivePointsMax = 1500;
            ActivePointsMin = 0;

            ActiveMovePointRangeMid = 30;
            ActiveMovePointRangeMin = 10;

            Discard();
        }
    }
}