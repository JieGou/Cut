// ------------------------------------------------
//                  LineManagementAssistant
// Copyright 2012-2013, Chengyong Yang & Changhai Gu. 
//               All rights reserved.
// ------------------------------------------------
//	ArxWrapper.h
//	written by Changhai Gu
// ------------------------------------------------
// $File:\\LineManageAssitant\main\source\ARX\ArxCustomObject.cpp $
// $Author: Changhai Gu $
// $DateTime: 2013/1/12 06:13:00
// $Revision: #1 $
// ------------------------------------------------

#include <ArxWrapper.h>
#include <ArxCustomObject.h>
#include <LMAUtils.h>
#include <LineEntryData.h>

namespace com
{

namespace guch
{

namespace assistant
{

namespace arx
{

void LMADbObjectManager::RegisterClass()
{
	LMALineDbObject::rxInit();
	LineEntry::rxInit();
}

void LMADbObjectManager::UnRegisterClass()
{
	deleteAcRxClass(LMALineDbObject::desc());
	deleteAcRxClass(LineEntry::desc());
}

ACRX_DXF_DEFINE_MEMBERS(LMALineDbObject, AcDb3dSolid, 
AcDb::kDHL_CURRENT, AcDb::kMReleaseCurrent, 
0,
    LMALineDbObject, LMA);

LMALineDbObject::LMALineDbObject()
	: mLineID(-1)
	,mSequenceNO(-1)
	,mLineEntry(NULL)
	,mStartPoint(AcGePoint3d::kOrigin)
	,mEndPoint(AcGePoint3d::kOrigin)
	,mAlignedDim(NULL)
{
};

Acad::ErrorStatus LMALineDbObject::Init()
{
	if( mLineEntry )
	{
		LOG(L"配置线段为空，寻找配置信息");
	}

	return CreatePipe();
}

// Gets the value of the line ID member.
//
Acad::ErrorStatus
	LMALineDbObject::getLineID(Adesk::Int32& lineId)
{
    // Tells AutoCAD a read operation is taking place.
    //
    assertReadEnabled();
    lineId = mLineID;
    return Acad::eOk;
}

// Sets the value of the line ID member.
//
Acad::ErrorStatus
LMALineDbObject::setLineID(Adesk::Int32 lineId)
{
    // Triggers openedForModify notification.
    //
    assertWriteEnabled();
    mLineID = lineId;
    return Acad::eOk;
}

// Gets the value of the line ID member.
//
Acad::ErrorStatus
	LMALineDbObject::getPointSeqNO(Adesk::Int32& pointSeqNO)
{
    // Tells AutoCAD a read operation is taking place.
    //
    assertReadEnabled();
    pointSeqNO = mSequenceNO;
    return Acad::eOk;
}

// Sets the value of the line ID member.
//
Acad::ErrorStatus
LMALineDbObject::setPointSeqNO(Adesk::Int32 pointSeqNO)
{
    // Triggers openedForModify notification.
    //
    assertWriteEnabled();
    mSequenceNO = pointSeqNO;
    return Acad::eOk;
}

// Files data in from a DWG file.
//
Acad::ErrorStatus
LMALineDbObject::dwgInFields(AcDbDwgFiler* pFiler)
{
    assertWriteEnabled();

    AcDb3dSolid::dwgInFields(pFiler);
    // For wblock filing we wrote out our owner as a hard
    // pointer ID so now we need to read it in to keep things
    // in sync.
    //
    if (pFiler->filerType() == AcDb::kWblockCloneFiler) {
        AcDbHardPointerId id;
        pFiler->readItem(&id);
    }

	Adesk::UInt32 lineID;
    pFiler->readItem(&lineID);
	mLineID = (UINT)lineID;

	Adesk::UInt32 seqNO;
	pFiler->readItem(&seqNO);
	mSequenceNO = (UINT)seqNO;

	pFiler->readPoint3d(&mStartPoint);
	pFiler->readPoint3d(&mEndPoint);
	
	CString filename;
	dbToStr(this->database(),filename);

#ifdef DEBUG
	acutPrintf(L"\n从DWG文件【%s】得到管线线段实体 ID【%d】序列号【%d】 起点 X:【%lf】Y:【%lf】Z:【%lf】 终点 X:【%lf】Y:【%lf】Z:【%lf】.",
					filename.GetBuffer(),mLineID,mSequenceNO,
					mStartPoint.x,mStartPoint.y,mStartPoint.z,
					mEndPoint.x,mEndPoint.y,mEndPoint.z);
#endif

	LineEntryFileManager::RegisterLineSegment(filename.GetBuffer(),this,mLineID,mSequenceNO,
												mStartPoint,mEndPoint);

    return pFiler->filerStatus();
}

// Files data out to a DWG file.
//
Acad::ErrorStatus
LMALineDbObject::dwgOutFields(AcDbDwgFiler* pFiler) const
{
    assertReadEnabled();

    AcDb3dSolid::dwgOutFields(pFiler);
    // Since objects of this class will be in the Named
    // Objects Dictionary tree and may be hard referenced
    // by some other object, to support wblock we need to
    // file out our owner as a hard pointer ID so that it
    // will be added to the list of objects to be wblocked.
    //
    if (pFiler->filerType() == AcDb::kWblockCloneFiler)
        pFiler->writeHardPointerId((AcDbHardPointerId)ownerId());

    pFiler->writeItem(Adesk::UInt32(mLineID));
	pFiler->writeItem(Adesk::UInt32(mSequenceNO));

	pFiler->writeItem(mStartPoint);
	pFiler->writeItem(mEndPoint);

	CString filename;
	dbToStr(this->database(),filename);

#ifdef DEBUG
	acutPrintf(L"\n从保存管线线段实体 ID【%d】序列号【%d】 起点 X:【%lf】Y:【%lf】Z:【%lf】 终点 X:【%lf】Y:【%lf】Z:【%lf】到DWG文件【%s】.",
					mLineID,mSequenceNO,
					mStartPoint.x,mStartPoint.y,mStartPoint.z,
					mEndPoint.x,mEndPoint.y,mEndPoint.z,
					filename.GetBuffer());
#endif

    return pFiler->filerStatus();
}

// Files data in from a DXF file.
//
Acad::ErrorStatus
LMALineDbObject::dxfInFields(AcDbDxfFiler* pFiler)
{
    assertWriteEnabled();

    Acad::ErrorStatus es;
    if ((es = AcDb3dSolid::dxfInFields(pFiler))
        != Acad::eOk)
    {
        return es;
    }

    // Check if we're at the right subclass getLineID marker.
    //
    if (!pFiler->atSubclassData(_T("LMALineDbObject"))) {
        return Acad::eBadDxfSequence;
    }

    struct resbuf inbuf;
    while (es == Acad::eOk) {
        if ((es = pFiler->readItem(&inbuf)) == Acad::eOk) {

			switch ( inbuf.restype )
			{
				case AcDb::kDxfInt32:
					mLineID = inbuf.resval.rint;
				case AcDb::kDxfInt32 + 1:
					mSequenceNO = inbuf.resval.rint;
			}
        }
    }

    return pFiler->filerStatus();
}

// Files data out to a DXF file.
//
Acad::ErrorStatus
LMALineDbObject::dxfOutFields(AcDbDxfFiler* pFiler) const
{
    assertReadEnabled();

    AcDb3dSolid::dxfOutFields(pFiler);
    pFiler->writeItem(AcDb::kDxfSubclass, _T("LMALineDbObject"));
    pFiler->writeItem(AcDb::kDxfInt32, mLineID);
	pFiler->writeItem(AcDb::kDxfInt32 + 1, mSequenceNO);

    return pFiler->filerStatus();
}

Acad::ErrorStatus LMALineDbObject::CreatePipe()
{
	acutPrintf(L"开始绘制管体\n");

	//得到线段的长度
	double length = mStartPoint.distanceTo(mEndPoint);
	if( length < 0.1 )
		return Acad::eInvalidInput;

	acutPrintf(L"得到管体高度%lf\n",length);

	//绘制圆柱体
	this->createFrustum(length,mRadius,mRadius,mRadius);

	//得到线段与Z轴的垂直向量
	AcGeVector3d line3dVector(mEndPoint.x - mStartPoint.x,mEndPoint.y - mStartPoint.y, mEndPoint.z-mStartPoint.z);
	AcGeVector3d rotateVctor = line3dVector.crossProduct(AcGeVector3d::kZAxis);

	//得到旋转的角度
	double angle = -line3dVector.angleTo(AcGeVector3d::kZAxis);
	acutPrintf(L"得到旋转角度%lf\n",angle);

	//进行旋转
	AcGeMatrix3d rotateMatrix = AcGeMatrix3d::rotation( angle, rotateVctor, AcGePoint3d::kOrigin);
	transformBy(rotateMatrix);
	
	//得到线段的中心点
	AcGePoint3d center(mStartPoint.x + mEndPoint.x, mStartPoint.y + mEndPoint.y, mStartPoint.z + mEndPoint.z); 
	center /= 2;
	acutPrintf(L"得到中心点[%lf][%lf][%lf]\n",center.x,center.y,center.z);

	//进行偏移
	AcGeMatrix3d moveMatrix;
	moveMatrix.setToTranslation(AcGeVector3d(center.x,center.y,center.z));

	transformBy(moveMatrix);

	CreateDimensions();

#ifdef DEBUG
	//acutPrintf(L"插入中心线，用于矫正");

	//AcDbLine *pLine = new AcDbLine(start, end);
    //ArxWrapper::PostToModelSpace(pLine,mLayerName);
#endif

	return Acad::eOk;
}

Acad::ErrorStatus LMALineDbObject::CreateDimensions()
{
	//创建标注体
    mAlignedDim = new AcDbAlignedDimension;

	//得到线段长度
	double length = mStartPoint.distanceTo(mEndPoint);
	if( length < 0.1 )
		return Acad::eInvalidInput;

#ifdef DEBUG
	acutPrintf(L"\n标注长度为【%lf】",length);
#endif

	static const double dimTextOff = mRadius * 2;

	//首先在原点处，沿X轴方向标注
	mAlignedDim->setXLine1Point(AcGePoint3d::kOrigin);
    mAlignedDim->setXLine2Point(AcGePoint3d(length,0,0));
	mAlignedDim->setTextPosition(AcGePoint3d(length/2,dimTextOff,0));

    // dimLinePt automatically set from where text was placed,
    // unless you deliberately set the dimLinePt
    //dim->setHorizontalRotation(getDimHorizRotation());

	//设置标注字的配置
    mAlignedDim->useSetTextPosition();    // make text go where user picked
    mAlignedDim->setDatabaseDefaults();

	//首先旋转到Z轴出
	AcGeMatrix3d zMatrix = AcGeMatrix3d::rotation( -1.57, AcGeVector3d::kYAxis, AcGePoint3d::kOrigin);
	mAlignedDim->transformBy(zMatrix);

	//得到线段与Z轴的垂直向量
	AcGeVector3d line3dVector(mEndPoint.x - mStartPoint.x,mEndPoint.y - mStartPoint.y, mEndPoint.z-mStartPoint.z);
	AcGeVector3d rotateVctor = line3dVector.crossProduct(AcGeVector3d::kZAxis);

	//得到旋转的角度
	double angle = -line3dVector.angleTo(AcGeVector3d::kZAxis);
	acutPrintf(L"得到旋转角度%lf\n",angle);

	//进行旋转
	AcGeMatrix3d rotateMatrix = AcGeMatrix3d::rotation( angle, rotateVctor, AcGePoint3d::kOrigin);
	mAlignedDim->transformBy(rotateMatrix);

	//进行偏移
	AcGeMatrix3d moveMatrix;
	moveMatrix.setToTranslation(AcGeVector3d(mStartPoint.x,mStartPoint.y,mStartPoint.z));

	mAlignedDim->transformBy(moveMatrix);

	//添加到模型空间
	ArxWrapper::PostToModelSpace(mAlignedDim,L"0");

	return Acad::eOk;
}

} // end of arx

} // end of assistant

} // end of guch

} // end of com
