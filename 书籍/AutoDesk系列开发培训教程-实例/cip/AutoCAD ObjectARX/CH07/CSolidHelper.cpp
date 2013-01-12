// (C) Copyright 2002-2007 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
#if defined(_DEBUG) && !defined(AC_FULL_DEBUG)
#error _DEBUG should not be defined except in internal Adesk debug builds
#endif

#include "StdAfx.h"
#include "CSolidHelper.h"
  


CSolidHelper::CSolidHelper(void)
{
}

CSolidHelper::~CSolidHelper(void)
{
}


//-------------------------------------------------------------------------------------------
	// 
	//  功能：  添加实体到数据库
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：
	//          
	//
	//----------------------------------------------------------------------------------------------
AcDbObjectId CSolidHelper::AddEntityToDatabase(AcDbEntity *pEnt, ACHAR *spc, AcDbDatabase *pDb)
{
	// Adding entity in to the Database, by default it will add in to the 
	AcDbObjectId outObjId=AcDbObjectId::kNull;
	Acad::ErrorStatus es;
	if(NULL != spc && NULL != pEnt && NULL != pDb)
	{
		AcDbBlockTablePointer pBT(pDb->blockTableId(),AcDb::kForRead);

		if(Acad::eOk == pBT.openStatus())
		{
			AcDbObjectId recordId=AcDbObjectId::kNull;
			pBT->getAt(spc,recordId);
			AcDbBlockTableRecordPointer pBtr(recordId,AcDb::kForWrite);
			if(Acad::eOk == pBtr.openStatus())
			{
				es = pBtr->appendAcDbEntity(outObjId,pEnt);
			}
		}
	}
	return outObjId;
}
//-------------------------------------------------------------------------------------------
	// 
	//  功能：  基于AcDbPolyline拉伸三维实体
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：
	//          
	//
	//----------------------------------------------------------------------------------------------
 Acad::ErrorStatus
CSolidHelper::extrudePoly(AcDbPolyline* pPoly, double height, AcDbObjectId& savedExtrusionId)
{
    Acad::ErrorStatus es = Acad::eOk;

    // Explode to a set of lines
    //
    AcDbVoidPtrArray lines;
    pPoly->explode(lines);

    // Create a region from the set of lines.
    //
    AcDbVoidPtrArray regions;
    AcDbRegion::createFromCurves(lines, regions);
    assert(regions.length() == 1);
    AcDbRegion *pRegion = AcDbRegion::cast((AcRxObject*)regions[0]);
    assert(pRegion != NULL);

    // Extrude the region to create a solid.
    //
    AcDb3dSolid *pSolid = new AcDb3dSolid;
    assert(pSolid != NULL);
    es  =  pSolid->extrude(pRegion, height, 0.0);

    for (int i = 0; i < lines.length(); i++) {
        delete (AcRxObject*)lines[i];
    }
    for (int ii = 0; ii < regions.length(); ii++) {
        delete (AcRxObject*)regions[ii];
    }
	if(Acad::eOk == es)
	{

    savedExtrusionId = CSolidHelper::AddEntityToDatabase(pSolid);
    pSolid->close();
	}
	else
		delete pSolid;

    return Acad::eOk;
}
