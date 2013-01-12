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
class CCH03App : public AcRxArxApp {

public:
	CCH03App () : AcRxArxApp () {}

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
	//  功能： 向层表中添加记录（创建新层)
	//                 
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH03.AddLayer command (do not rename)
	static void CGDCH03AddLayer(void)
	{
		//先声明一个空的层表指针
		AcDbLayerTable *pLayerTbl;  
		//通过当前图形数据库获取层表对象指针
		//打开层表为写入状态
		acdbHostApplicationServices()->workingDatabase()  ->getSymbolTable(pLayerTbl, AcDb::kForWrite); 

		//判断层是否已经存在
		if(!pLayerTbl->has(_T("MyLayer"))) {   
			//新层表记录
			AcDbLayerTableRecord *pLayerTblRcd=  new AcDbLayerTableRecord;
			pLayerTblRcd->setName(_T("MyLayer"));   //设定图层名
			pLayerTblRcd->setIsFrozen(0);   // 图层解冻
			pLayerTblRcd->setIsOff(0);      // 打开图层
			pLayerTblRcd->setVPDFLT(0);   // 使用默认视口
			pLayerTblRcd->setIsLocked(0);   // 图层解锁
			// AcCmColor是ACAD颜色管理类
			AcCmColor color;    
			color.setColorIndex(1);  
			// 图层颜色为红色    
			pLayerTblRcd->setColor(color);  
			// 为给新图层设置线型，要得到线型表记录的ID。
			//  以下的代码演示如何得到并操作记录ID
			AcDbLinetypeTable *pLinetypeTbl;
			AcDbObjectId ltId;
			acdbHostApplicationServices()->workingDatabase()->getSymbolTable(pLinetypeTbl, AcDb::kForRead);
			if ((pLinetypeTbl->getAt(_T("DASHED"), ltId))!= Acad::eOk)	{  
				acutPrintf(_T("\n未发现DASHED线型使用CONTINUOUS线型"));
				// 每一个不完全空的图形数据库的线型表中都有线型名为CONTINUOUS 的默认记录
				pLinetypeTbl->getAt(_T("CONTINUOUS"), ltId);
			}
			pLinetypeTbl->close();
			pLayerTblRcd->setLinetypeObjectId(ltId);
			pLayerTbl->add(pLayerTblRcd);
			pLayerTblRcd->close();
			pLayerTbl->close();
		} else {
			pLayerTbl->close();
			acutPrintf(_T("\n层已经存在"));
		}

	}
public:
	//-------------------------------------------------------------------------------------------
	// 
	//  功能： 遍历线型表中的所有记录，获得并打印线型名
	//                 
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH03.IterateLinetype command (do not rename)
	static void CGDCH03IterateLinetype(void)
	{
		// 获取线型表
		AcDbLinetypeTable *pLinetypeTbl;
		acdbHostApplicationServices()->workingDatabase()
			->getSymbolTable(pLinetypeTbl, AcDb::kForRead);

		// 创建线性表的遍历器
		AcDbLinetypeTableIterator *pLtIterator;
		pLinetypeTbl->newIterator(pLtIterator);

		// 遍历线性表，输出每个线性表记录的名称
		AcDbLinetypeTableRecord *pLtTableRcd;
		const TCHAR *pLtName;
		for (; !pLtIterator->done(); pLtIterator->step()) {
			pLtIterator->getRecord(pLtTableRcd, AcDb::kForRead);
			pLtTableRcd->getName(pLtName);
			pLtTableRcd->close();
			acutPrintf(_T("\n线型名称:  %s"), pLtName);
		}
		// 删除线性表遍历器并关闭线性表
		delete pLtIterator;
		pLinetypeTbl->close();

	}
} ;

//-----------------------------------------------------------------------------
IMPLEMENT_ARX_ENTRYPOINT(CCH03App)


ACED_ARXCOMMAND_ENTRY_AUTO(CCH03App, CGDCH03, AddLayer, AddLayer, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH03App, CGDCH03, IterateLinetype, IterateLinetype, ACRX_CMD_TRANSPARENT, NULL)
