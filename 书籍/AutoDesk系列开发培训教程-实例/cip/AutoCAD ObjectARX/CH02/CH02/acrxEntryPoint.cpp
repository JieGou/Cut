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
class CCH02App : public AcRxArxApp {

public:
	CCH02App () : AcRxArxApp () {}

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
	//  功能： 获取用户输入
	//                 
	//
	//  作者：Zhao C.X.
	//
	// 日期：200709
	//
	//  历史：
	//      
	//----------------------------------------------------------------------------------------------
	// - CGDCH02.GetData command (do not rename)
	static void CGDCH02GetData(void)
	{
		ads_point insertPt;
		if(acedGetPoint(NULL,_T("\n请指定同心圆的圆心:"),insertPt)!=RTNORM) return;

		double radius;		//输入圆的半径

		int nCount = 3;
		acedInitGet(RSG_NONEG + RSG_NOZERO, NULL);	//不允许0和负数
		acedGetInt(_T("\n请输入要绘制圆的个数<3>:"), &nCount);

		ads_real fDist = 20.0;	//圆与圆的间距
		acedInitGet(RSG_NONEG + RSG_NOZERO, NULL);
		acedGetDist(NULL, _T("\n请输入圆与圆的间距<20.0>:"), &fDist);
		//使用获取的数据。。。
	}
public:
 //-------------------------------------------------------------------------------------------
	// 
	//  功能： 获取关键字，并处理啊选择集
	//                 
	//
	//  作者：Zhao C.X.
	//
	// 日期：200709
	//
	//  历史：
	//      
	//----------------------------------------------------------------------------------------------
	// - CGDCH02.GetKWord command (do not rename)
	static void CGDCH02GetKWord(void)
	{
		//用户输入实体的颜色
		AcCmColor acColor;	
		TCHAR szKword [132];
		szKword[0] = _T('R');	//给szKword一个默认值R
		szKword[1] = _T('\0');
		//初始化关键字表，大写字母是关键字的简写
		acedInitGet(0, _T("Red Green Blue"));
		int nReturn;
		//取得用户输入的关键字
		nReturn = acedGetKword(_T("\n请选择实体的颜色Red/Green/Blue<Red>: "), szKword);
		if (nReturn == RTNORM)	//如果得到合理的关键字
		{
			if (_tcscmp(szKword, _T("Red")) == 0)
				acColor.setColorIndex(1);	//红色
			else if (_tcscmp(szKword, _T("Blue")) == 0)
				acColor.setColorIndex(5);	//蓝色
			else
				acColor.setColorIndex(3);	//绿色
		}
		else if (nReturn == RTNONE)	//如果用户输入为空值
		{
			acColor.setColorIndex(1);		//默认红色
		}

		//获取选择集
		ads_name ssname;
		acutPrintf(_T("\n请选择要改变颜色的实体："));
		acedSSGet(NULL, NULL, NULL, NULL, ssname);
		//得到选择集的实体个数，如果为0，则退出
		long ssLen = 0;
		acedSSLength(ssname, &ssLen);
		if (ssLen == 0)
			return;
		//遍历选择集
		ads_name ent;						
		AcDbEntity* pEnt = NULL;			
		AcDbObjectId objId;
		long i;
		for(i=0; i<ssLen; i++)
		{
			acedSSName(ssname, i, ent);		
			acdbGetObjectId(objId, ent);	

			if(Acad::eOk == acdbOpenObject(pEnt, objId, AcDb::kForWrite))
			{
				pEnt->setColor(acColor);	//设置实体的颜色
				pEnt->close();
			}
		}
		//释放选择集
		acedSSFree(ssname);
	}
public:
    //-------------------------------------------------------------------------------------------
	// 
	//  功能： 基于过滤条件创建选择集
	//                 
	//
	//  作者：Zhao C.X.
	//
	// 日期：200709
	//
	//  历史：  
	//             
	//----------------------------------------------------------------------------------------------

	// - CGDCH02.SSFilter command (do not rename)
	static void CGDCH02SSFilter(void)
	{
				struct resbuf eb1,eb2,eb3;
		TCHAR sbuf1[20];
		TCHAR sbuf2[20];
		ads_name ssname;
		eb1.restype=0; 		//实体类型
		_tcscpy(sbuf1, _T("LINE"));	//直线
		eb1.resval.rstring=sbuf1;
		eb2.restype=8;			//图层名
		_tcscpy(sbuf2, _T("0"));
		eb2.resval.rstring=sbuf2;
		eb3.restype=62;		//实体颜色
		eb3.resval.rint=1;	//红色
		// eb3.resval.rint=256;	//随层颜色
		eb3.rbnext=NULL;
		//增加另外两个过滤条件建立链表
		eb1.rbnext=&eb2;		
		eb2.rbnext=&eb3;
		//检索在0图层上所有红色的直线，选择集放在ssname中
		acutPrintf(_T("\n请选择实体，只有0图层上的红色的直线才能被选中。"));
		acedSSGet(NULL,NULL,NULL,&eb1,ssname);
		// 处理选择集
		long ssLen = 0;
		acedSSLength(ssname, &ssLen);
		if (ssLen == 0)
			return;
		//遍历选择集
		ads_name ent;						
		AcDbEntity* pEnt = NULL;			
		AcDbObjectId objId;
		for(long i=0; i<ssLen; i++)
		{
			acedSSName(ssname, i, ent);		
			acdbGetObjectId(objId, ent);	
			if(Acad::eOk == acdbOpenObject(pEnt, objId, AcDb::kForWrite))
			{
				pEnt->setColorIndex(2);	//设置实体的颜色
				pEnt->close();
			}
		}

		//释放选择集
		acedSSFree(ssname);
	}
} ;

//-----------------------------------------------------------------------------
IMPLEMENT_ARX_ENTRYPOINT(CCH02App)

ACED_ARXCOMMAND_ENTRY_AUTO(CCH02App, CGDCH02, GetData, GetData, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH02App, CGDCH02, GetKWord, GetKWord, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH02App, CGDCH02, SSFilter, SSFilter, ACRX_CMD_TRANSPARENT, NULL)
