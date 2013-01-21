// ------------------------------------------------
//                  LineManagementAssistant
// Copyright 2012-2013, Chengyong Yang & Changhai Gu. 
//               All rights reserved.
// ------------------------------------------------
//	ArxWrapper.h
//	written by Changhai Gu
// ------------------------------------------------
// $File:\\LineManageAssitant\main\header\ArxCustomObject.h $
// $Author: Changhai Gu $
// $DateTime: 2013/1/9 01:35:46 $
// $Revision: #1 $
// ------------------------------------------------

#if defined(_DEBUG) && !defined(AC_FULL_DEBUG)
#error _DEBUG should not be defined except in internal Adesk debug builds
#endif

#include <rxobject.h>
#include <rxregsvc.h>
#include <aced.h>
#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include "tchar.h"
#include <dbobjptr.h>

//3D Object
#include <dbsol3d.h>

#include <string>

#include <LineEntryData.h>

using namespace com::guch::assistant::data;

namespace com
{

namespace guch
{

namespace assistant
{

namespace arx
{

class LMADbObjectManager
{
public:

	static void RegisterClass();
	static void UnRegisterClass();
};

class LMALineDbObject : public AcDb3dSolid
{
public:
    ACRX_DECLARE_MEMBERS(LMALineDbObject);

    LMALineDbObject();
    LMALineDbObject(const Adesk::Int32& id,
					const Adesk::Int32& seqNO,
					const AcGePoint3d& start,
					const AcGePoint3d& end,
					const double& radius,
					LineEntry* lineEntry)
		: mLineID(id)
		, mSequenceNO(seqNO)
		, mStartPoint(start)
		, mEndPoint(end)
		, mRadius(radius)
		, mLineEntry(lineEntry)
	{
		Init();
	}

    Acad::ErrorStatus         getLineID     (Adesk::Int32&);
    Acad::ErrorStatus         setLineID     (Adesk::Int32);

	Acad::ErrorStatus         getPointSeqNO     (Adesk::Int32&);
    Acad::ErrorStatus         setPointSeqNO     (Adesk::Int32);

    virtual Acad::ErrorStatus dwgInFields (AcDbDwgFiler*);
    virtual Acad::ErrorStatus dwgOutFields(AcDbDwgFiler*)
        const;
    virtual Acad::ErrorStatus dxfInFields (AcDbDxfFiler*);
    virtual Acad::ErrorStatus dxfOutFields(AcDbDxfFiler*)
        const;

protected:

	// create the line pipe
	Acad::ErrorStatus Init();

	// creat the 3d pipe
	Acad::ErrorStatus CreatePipe();

	// create the dimensions
	Acad::ErrorStatus CreateDimensions();

private:

	//Store in database
	Adesk::Int32 mLineID;

	//Identify the index in the line
	Adesk::Int32 mSequenceNO;

	//Identify the line
	LineEntry* mLineEntry;

	//the bottom
	AcGePoint3d mStartPoint;

	//the top
	AcGePoint3d mEndPoint;

	//the outter radius
	double mRadius;

	//dimension
	AcDbAlignedDimension* mAlignedDim;
};

} // end of arx

} // end of assistant

} // end of guch

} // end of com
