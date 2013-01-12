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
#include "tchar.h"
//-----------------------------------------------------------------------------
#define szRDS _RXST("CGD")

//-----------------------------------------------------------------------------
//----- ObjectARX EntryPoint
class CCH04App : public AcRxArxApp {

public:
	CCH04App () : AcRxArxApp () {}

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
	//  功能： 删除复杂实体-AcDb2dPolyline
	//                 
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	static void delete2dPolyline(AcDb2dPolyline* pPline)
	{ 
		//创建顶点遍历器
		AcDbObjectIterator* pIter=pPline->vertexIterator();
		AcDbEntity* pEnt; 
		for (; !pIter->done(); ) {
			//依次删除顶点
			pEnt=pIter->entity(); 
			pIter->step();
			delete pEnt;
		}
		//删除遍历器，删除polyline
		delete pIter;
		pPline->erase();
	}
   //-------------------------------------------------------------------------------------------
	// 
	//  功能： 遍历复杂实体-AcDb2dPolyline的子对象
	//                 
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
static void IteratePolyline(AcDb2dPolyline *pLine)
{
	//打开polyline对象
	//AcDb2dPolyline *pPline;
	//acdbOpenObject(pPline, plineId, AcDb::kForRead);
	//创建顶点遍历器指针
	AcDbObjectIterator *pVertIter= pLine->vertexIterator();
	//pPline->close();  
	AcDb2dVertex *pVertex;
	AcGePoint3d location;
	AcDbObjectId vertexObjId;
	//循环遍历polyline顶点
	for (int vertexNumber = 0; !pVertIter->done();	vertexNumber++, pVertIter->step())
	{
		vertexObjId = pVertIter->objectId();
		acdbOpenObject(pVertex, vertexObjId,AcDb::kForRead);
		location = pVertex->position();
		pVertex->close();
		acutPrintf(_T("顶点 #%d位置： %0.3f, %0.3f, %0.3f"), vertexNumber,location[X], location[Y], location[Z]);
	}
	//删除遍历器
	delete pVertIter;
}
   //-------------------------------------------------------------------------------------------
	// 
	//  功能： 创建或获取块定义的对象ID
	//                 
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
static void CreateBlockDef(AcDbObjectId &blockTableRecordId)
{

	//打开块表
	AcDbBlockTable *pBlockTable = NULL;
	acdbHostApplicationServices()->workingDatabase()
		->getSymbolTable(pBlockTable, AcDb::kForWrite);
	// 
	if(!pBlockTable->has (_T("MyBlockName")))
	{
		//新建块表记录并设置其名称
		AcDbBlockTableRecord *pBlockTableRec = new AcDbBlockTableRecord();
		pBlockTableRec->setName(_T("MyBlockName"));
		// 
		//添加新建块表记录到块表中
		pBlockTable->add(blockTableRecordId, pBlockTableRec);
		// 
		//新建实体对象并附加到块表记录中
		AcDbLine *pLine = new AcDbLine();
		AcDbObjectId lineId;
		pLine->setStartPoint(AcGePoint3d(0, 0, 0));
		pLine->setEndPoint(AcGePoint3d(6, 6, 0));
		pLine->setColorIndex(3);
		pBlockTableRec->appendAcDbEntity(lineId, pLine);
		pLine->close();
		// 创建一个圆
		AcDbCircle* pCircle = new AcDbCircle();
		pCircle->setCenter (AcGePoint3d(0, 0, 0));
		pCircle->setColorIndex(1);
		pCircle->setRadius (2.0);
		AcDbObjectId cirId;
		pBlockTableRec->setOrigin (AcGePoint3d(0, 0, 0));
		pBlockTableRec->appendAcDbEntity(cirId, pCircle);
		pCircle->close();//关闭实体对象

		//关闭块表记录对象
		pBlockTableRec->close();
	}
	else
	{
		pBlockTable->getAt (_T("MyBlockName"),blockTableRecordId);
	}
	//关闭块表
	pBlockTable->close();

}

	//-------------------------------------------------------------------------------------------
	// 
	//  功能： 创建直线对象
	//                 
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH04.AddLine command (do not rename)
	static void CGDCH04AddLine(void)
	{
		//新建实体对象：
		AcGePoint3d startPt(4.0, 2.0, 0.0);
		AcGePoint3d endPt(10.0, 7.0, 0.0);
		AcDbLine *pLine = new AcDbLine(startPt, endPt);
		//打开块表记录：
		AcDbBlockTable *pBlockTable;
		acdbHostApplicationServices()->workingDatabase()
			->getSymbolTable(pBlockTable, AcDb::kForRead);
		AcDbBlockTableRecord *pBlockTableRecord;
		pBlockTable->getAt(ACDB_MODEL_SPACE, pBlockTableRecord,
			AcDb::kForWrite);
		pBlockTable->close();
		//添加实体对象到块表记录：
		AcDbObjectId lineId;
		pBlockTableRecord->appendAcDbEntity(lineId, pLine);
		//关闭块表记录和实体：
		pBlockTableRecord->close();
		pLine->close();
		return ;

	}
public:
	//-------------------------------------------------------------------------------------------
	// 
	//  功能： 创建复杂实体-AcDb2dPolyline
	//                 
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH04.AddPolyline command (do not rename)
	static void CGDCH04AddPolyline(void)
	{
		//设置pline的顶点坐标
		AcGePoint3dArray ptArr;
		ptArr.setLogicalLength(4);
		for (int i = 0; i < 4; i++) {
			ptArr[i].set((double)(i/2), (double)(i%2), 0.0);
		}
		//新建AcDb2dPolyline对象
		AcDb2dPolyline *pPolyline = new AcDb2dPolyline(
			AcDb::k2dSimplePoly, ptArr, 0.0, Adesk::kTrue);
		//设置颜色
		pPolyline->setColorIndex(3);
		//
		//得到块表对象
		AcDbBlockTable *pBlockTable;
		acdbHostApplicationServices()->workingDatabase()
			->getSymbolTable(pBlockTable, AcDb::kForRead);
		// 
		//得到模型空间
		AcDbBlockTableRecord *pBlockTableRecord;
		pBlockTable->getAt(ACDB_MODEL_SPACE, pBlockTableRecord, AcDb::kForWrite);
		pBlockTable->close();
		// 
		//附加AcDb2dPolyline对象到模型空间
		AcDbObjectId plineObjId;
		pBlockTableRecord->appendAcDbEntity(plineObjId, pPolyline);
		pBlockTableRecord->close();
		//
		//设置图层
		pPolyline->setLayer(_T("0"));
		pPolyline->close();

	}
public:
//-------------------------------------------------------------------------------------------
	// 
	//  功能：编辑实体-修改颜色
	//                 
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH04.ChangeColor command (do not rename)
	static void CGDCH04ChangeColor(void)
	{
		//选择对象
		ads_name en;
		ads_point pt;
		acedEntSel( _T("\n选择实体: "), en, pt); 
		AcDbObjectId id;
		//转化ads_name为AcDbObjectId
		acdbGetObjectId(id,en);
		AcDbEntity * pEnt;
		//打开对象
		acdbOpenObject(pEnt, id, AcDb::kForRead );
		//将打开的对象转化为AcDbLine对象指针
		if(pEnt->isKindOf (AcDbLine::desc())){
			// 编辑对象 
			AcDbLine* pLine = AcDbLine::cast(pEnt);
			pLine->upgradeOpen ();
			pLine->setColorIndex (1);
		}
		//关闭对象
		pEnt->close();

	}
public:
//-------------------------------------------------------------------------------------------
	// 
	//  功能： 遍历复杂实体-AcDb2dPolyline的子对象
	//                 
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH04.ItePolyline command (do not rename)
	static void CGDCH04ItePolyline(void)
	{
		//选择对象
		ads_name en;
		ads_point pt;
		acedEntSel( _T("\n选择实体: "), en, pt); 
		AcDbObjectId id;
		//转化ads_name为AcDbObjectId
		acdbGetObjectId(id,en);
		AcDbEntity * pEnt;
		//打开对象
		acdbOpenObject(pEnt, id, AcDb::kForRead );
		//将打开的对象转化为AcDb2dPolyline对象指针
		if(pEnt->isKindOf (AcDb2dPolyline::desc())){
			// 编辑对象 
			AcDb2dPolyline* pLine = AcDb2dPolyline::cast(pEnt);
			IteratePolyline(pLine);
		}
		//关闭对象
		pEnt->close();
	}
public:
    //-------------------------------------------------------------------------------------------
	// 
	//  功能： 删除复杂实体-AcDb2dPolyline
	//                 
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH04.DelPolyline command (do not rename)
	static void CGDCH04DelPolyline(void)
	{
		//选择对象
		ads_name en;
		ads_point pt;
		acedEntSel( _T("\n选择实体: "), en, pt); 
		AcDbObjectId id;
		//转化ads_name为AcDbObjectId
		acdbGetObjectId(id,en);
		AcDbEntity * pEnt;
		//打开对象
		acdbOpenObject(pEnt, id, AcDb::kForWrite );
		//将打开的对象转化为AcDb2dPolyline对象指针
		if(pEnt->isKindOf (AcDb2dPolyline::desc())){
			// 编辑对象 
			AcDb2dPolyline* pLine = AcDb2dPolyline::cast(pEnt);
			delete2dPolyline(pLine);
			// 关闭对象
			pLine->close();
		}
		// 重绘制
		acedCommand(RTSTR,_T("regen"),RTNONE);

	}
public:
    //-------------------------------------------------------------------------------------------
	// 
	//  功能： 插入块
	//                 
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH04.InsertBlock command (do not rename)
	static void CGDCH04InsertBlock(void)
	{

		//选取块引用插入点
		AcGePoint3d basePoint;
		if (acedGetPoint(NULL, _T("\n输入插入点： "), asDblArray(basePoint)) != RTNORM)
			return;

		//创建一个有属性的块定义
		AcDbObjectId blockId;
		CreateBlockDef (blockId);
		// 
		//新建块引用对象
		AcDbBlockReference *pBlkRef = new AcDbBlockReference;
		// 
		//指定块引用所引用的块定义的ID
		pBlkRef->setBlockTableRecord(blockId);

		AcGeVector3d normal(0.0, 0.0, 1.0);
		// 
		//设置块引用的插入点、旋转角度和向量
		pBlkRef->setPosition(basePoint);
		pBlkRef->setRotation(0.0);
		pBlkRef->setNormal(normal);

		//打开当前数据库的模型空间
		AcDbBlockTable *pBlockTable;
		acdbHostApplicationServices()->workingDatabase()
			->getSymbolTable(pBlockTable, AcDb::kForRead);
		AcDbBlockTableRecord *pBlockTableRecord;
		pBlockTable->getAt(ACDB_MODEL_SPACE, pBlockTableRecord, AcDb::kForWrite);
		pBlockTable->close();
		//将创建的块引用附加到模型空间块表记录
		AcDbObjectId newEntId;
		pBlockTableRecord->appendAcDbEntity(newEntId, pBlkRef);
		pBlockTableRecord->close();

		pBlkRef->close();

	}
} ;

//-----------------------------------------------------------------------------
IMPLEMENT_ARX_ENTRYPOINT(CCH04App)

ACED_ARXCOMMAND_ENTRY_AUTO(CCH04App, CGDCH04, AddLine, AddLine, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH04App, CGDCH04, AddPolyline, AddPolyline, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH04App, CGDCH04, ChangeColor, ChangeColor, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH04App, CGDCH04, ItePolyline, ItePolyline, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH04App, CGDCH04, DelPolyline, DelPolyline, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH04App, CGDCH04, InsertBlock, InsertBlock, ACRX_CMD_TRANSPARENT, NULL)
