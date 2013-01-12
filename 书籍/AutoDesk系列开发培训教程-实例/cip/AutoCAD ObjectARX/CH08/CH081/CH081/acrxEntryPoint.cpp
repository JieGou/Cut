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
#include "CPipeAttribute.h"
#include <tchar.h>
//-----------------------------------------------------------------------------
#define szRDS _RXST("CS")

//-----------------------------------------------------------------------------
//----- ObjectARX EntryPoint
class CCH081App : public AcRxArxApp {

public:
	CCH081App () : AcRxArxApp () {}

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
	//  功能： 将从AcDbObject派生数据库对象保存到实体的扩展词典中
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：
	//          2007.10.08 修改 by Qin H.X.
	//
	//----------------------------------------------------------------------------------------------
	// - CSCH081.AddAttribute command (do not rename)
	static void CSCH081AddAttribute(void)
	{
		AcDbObjectId dictObjId,eId, attId;
		AcDbDictionary* pDict;
		//选择管道（多义线）
		ads_name en;
		ads_point pt;
		if (  acedEntSel(_T("\n选择管道（多义线）: "), en, pt)!= RTNORM)
		{        
			acutPrintf(_T("\n选择失败，退出: "));
			return ;
		}
		// 打开对象
		acdbGetObjectId(eId, en);
		AcDbEntity * pEnt;
		acdbOpenObject(pEnt, eId,  AcDb::kForWrite);
		if(!pEnt->isKindOf (AcDbPolyline::desc ()))
		{
			acutPrintf(_T("\n选择的不是管道（多义线），退出: " ));
			return ;
		}

		// 判断实体的扩展词典是否创建，如果没有则创建
		dictObjId = pEnt->extensionDictionary();
		if(	dictObjId ==  AcDbObjectId::kNull )
		{
			pEnt->createExtensionDictionary();
		}

		// 获取实体的扩展词典
		dictObjId = pEnt->extensionDictionary();
		pEnt->close();

		// 	判断词典中的属性是否创建	
		CPipeAttribute* pAttribute;
		acdbOpenObject(pDict, dictObjId, AcDb::kForWrite);
		pDict->getAt (_T("属性"),attId);
		if(attId!= AcDbObjectId::kNull )//如果已经创建则输出数据
		{
			acdbOpenObject(pAttribute, attId, AcDb::kForRead);
			acutPrintf(_T("\n管径：%4.2f " ),pAttribute->m_dRadius);
			acutPrintf(_T("\n壁厚：%4.2f " ),pAttribute->m_dThickness );
			acutPrintf(_T("\n埋深：%4.2f " ),pAttribute->m_dDeep );
			acutPrintf(_T("\n材质：%s " ),pAttribute->m_cMaterial  );
		}
		else
		{
			//没有则创建属性
			 pAttribute = new CPipeAttribute();
			pDict->setAt(_T("属性"), pAttribute, attId);
		}
		//关闭对象
		pDict->close();
		pAttribute->close();

	}
} ;

//-----------------------------------------------------------------------------
IMPLEMENT_ARX_ENTRYPOINT(CCH081App)

ACED_ARXCOMMAND_ENTRY_AUTO(CCH081App, CSCH081, AddAttribute, AddAttribute, ACRX_CMD_TRANSPARENT, NULL)
