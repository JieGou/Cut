// (C) Copyright 2002-2005 by Autodesk, Inc. 
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

//-----------------------------------------------------------------------------
//----- acrxEntryPoint.h
//-----------------------------------------------------------------------------
#include "StdAfx.h"
#include "resource.h"
#include"../CH082/CPipeLine.h"
//-----------------------------------------------------------------------------
#define szRDS _RXST("CS")

//-----------------------------------------------------------------------------
//----- ObjectARX EntryPoint
class CCH08UIApp : public AcRxArxApp {

public:
	CCH08UIApp () : AcRxArxApp () {}

	virtual AcRx::AppRetCode On_kInitAppMsg (void *pkt) {
		// TODO: Load dependencies here

		// You *must* call On_kInitAppMsg here
		AcRx::AppRetCode retCode =AcRxArxApp::On_kInitAppMsg (pkt) ;
		
		// TODO: Add your initialization code here

		return (retCode) ;
	}

	virtual AcRx::AppRetCode On_kUnloadAppMsg (void *pkt) {
		// TODO: Add your code here

		// You *must* call On_kUnloadAppMsg here
		AcRx::AppRetCode retCode =AcRxArxApp::On_kUnloadAppMsg (pkt) ;

		// TODO: Unload dependencies here

		return (retCode) ;
	}

	virtual void RegisterServerComponents () {
	}

public:
	//-------------------------------------------------------------------------------------------
	// 
	//  功能： 添加实体到数据库中
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：
	//          2007.10.08 修改 by Qin H.X.
	//
	//----------------------------------------------------------------------------------------------
static Acad::ErrorStatus  AddToDatabase(AcDbObjectId &objId, AcDbEntity* pEntity)
{
    AcDbBlockTable *pBlockTable;
    AcDbBlockTableRecord *pSpaceRecord;

    acdbHostApplicationServices()->workingDatabase()
        ->getSymbolTable(pBlockTable, AcDb::kForRead);
    pBlockTable->getAt(ACDB_MODEL_SPACE, pSpaceRecord,
        AcDb::kForWrite);

    pSpaceRecord->appendAcDbEntity(objId, pEntity);

    pBlockTable->close();
    pEntity->close();
    pSpaceRecord->close();

    return Acad::eOk;
}
	//-------------------------------------------------------------------------------------------
	// 
	//  功能： 创建自定义实体对象实例
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：
	//          2007.10.08 修改 by Qin H.X.
	//
	//----------------------------------------------------------------------------------------------
	// - CSCH08UI.AddPipe command (do not rename)
	static void CSCH08UIAddPipe(void)
	{
		// 创建管道类
		CPipeLine *pPipeline = new CPipeLine();
		AcGePoint2d pt0(0,0);
		AcGePoint2d pt1(10,10);
		AcGePoint2d pt2(20,0);
		AcGePoint2d pt3(30,10);
		AcGePoint2d pt4(40,0);

		pPipeline->addVertexAt (0,pt0);
		pPipeline->addVertexAt (1,pt1);
		pPipeline->addVertexAt (2,pt2);
		pPipeline->addVertexAt (3,pt3);
		pPipeline->addVertexAt (4,pt4);
		pPipeline->setElevation(2.6);
		//pPipeline->setClosed (true);
		AcDbObjectId idPipe;
		// 将管道类添加到数据库
		AddToDatabase(idPipe,pPipeline);

	}
} ;

//-----------------------------------------------------------------------------
IMPLEMENT_ARX_ENTRYPOINT(CCH08UIApp)

ACED_ARXCOMMAND_ENTRY_AUTO(CCH08UIApp, CSCH08UI, AddPipe, AddPipe, ACRX_CMD_TRANSPARENT, NULL)
