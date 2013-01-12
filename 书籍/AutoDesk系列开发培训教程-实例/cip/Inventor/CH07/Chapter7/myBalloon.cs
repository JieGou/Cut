using System;
using System.Collections.Generic;
using System.Text;
using Inventor;

namespace Chapter7
{
    class myBalloon
    {
        public void ChangeBalloons(Inventor.Application ThisApplication)
        {
            DrawingDocument drawDoc;
            drawDoc = (DrawingDocument)ThisApplication.ActiveDocument;

            Sheet sheet;
            sheet = drawDoc.ActiveSheet;

            long balloonCount;
            balloonCount = 1;

            foreach(Balloon balloon in sheet.Balloons)
            {
                balloon.SetBalloonType(BalloonTypeEnum.kHexagonBalloonType, null);
                balloon.PlacementDirection = BalloonDirectionEnum.kBottomDirection;

                long valueSetCount;
                valueSetCount = 1;

                foreach(BalloonValueSet balloonValueSet in balloon.BalloonValueSets)
                {
                    balloonValueSet.OverrideValue = string.Format("Balloon {0} : ValueSet {1}", balloonCount, valueSetCount);
                    valueSetCount = valueSetCount + 1;
                }
                balloonCount = balloonCount + 1;
            }
        }
    }
}
