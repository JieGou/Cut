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
class CCH05App : public AcRxArxApp {

public:
	CCH05App () : AcRxArxApp () {}

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
	//  功能： 输出扩展数据所支持的结果缓冲数据
	//                 
	//
	//  作者： 
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	static void printList(struct resbuf* pRb){
    int rt, i;
    TCHAR buf[133];

    for (i = 0;pRb != NULL;i++, pRb = pRb->rbnext) {
        if (pRb->restype < 1010) {
            rt = RTSTR;
        } else if (pRb->restype < 1040) {
            rt = RT3DPOINT;
        } else if (pRb->restype < 1060) {
            rt = RTREAL;
        } else if (pRb->restype < 1071) {
            rt = RTSHORT;
        } else if (pRb->restype == 1071) {
            rt = RTLONG;
        } else {// 
            rt = pRb->restype; //未知类型
        }

        switch (rt) {
        case RTSHORT:
            if (pRb->restype == RTSHORT) {
                acutPrintf( _T("短整类型数据（RTSHORT）为 : %d\n"), pRb->resval.rint);
            } else {
                acutPrintf(_T("(%d . %d)\n"), pRb->restype, pRb->resval.rint);
            };
            break;

        case RTREAL:
            if (pRb->restype == RTREAL) {
                acutPrintf(_T("浮点类型数据（RTREAL）为: %0.3f\n"),
                    pRb->resval.rreal);
            } else {
                acutPrintf(_T("(%d . %0.3f)\n"), pRb->restype,
                    pRb->resval.rreal);
            };
            break;

        case RTSTR:
            if (pRb->restype == RTSTR) {
                acutPrintf(_T("字符类型数据（RTSTR）为: %s\n"),
                    pRb->resval.rstring);
            } else {
                acutPrintf(_T("(%d . \"%s\")\n"), pRb->restype,
                    pRb->resval.rstring);
            };
            break;

        case RT3DPOINT:
            if (pRb->restype == RT3DPOINT) {
                acutPrintf(
                    _T("几何点类型数据（RT3DPOINT）为: %0.3f, %0.3f, %0.3f\n"),
                    pRb->resval.rpoint[X],
                    pRb->resval.rpoint[Y],
                    pRb->resval.rpoint[Z]);
            } else {
                acutPrintf(_T("(%d %0.3f %0.3f %0.3f)\n"),
                    pRb->restype,
                    pRb->resval.rpoint[X],
                    pRb->resval.rpoint[Y],
                    pRb->resval.rpoint[Z]);
            }
            break;

        case RTLONG:
            acutPrintf(_T("长整类型数据（RTLONG）为: %dl\n"), pRb->resval.rlong);
            break;
        }

        if ((i == 23) && (pRb->rbnext != NULL)) {
            i = 0;
            acedGetString(0,
                _T("回车继续..."), buf);
        }
    }
}
    //-------------------------------------------------------------------------------------------
	// 
	//  功能： 输出扩展记录所支持的结果缓冲数据
	//                 
	//
	//  作者： 
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
static	void printListXRecord(struct resbuf* pBuf)
{
    int rt, i;
    TCHAR buf[133];

    for (i = 0;pBuf != NULL;i++, pBuf = pBuf->rbnext) {
        if (pBuf->restype < 0)
            // Entity name (or other sentinel)
            rt = pBuf->restype;
        else if (pBuf->restype < 10)
            rt = RTSTR;
        else if (pBuf->restype < 38)
            rt = RT3DPOINT;
        else if (pBuf->restype < 60)
            rt = RTREAL;
        else if (pBuf->restype < 80)
            rt = RTSHORT;
        else if (pBuf->restype < 100)
            rt = RTLONG;
        else if (pBuf->restype < 106)
            rt = RTSTR;
        else if (pBuf->restype < 148)
            rt = RTREAL;
        else if (pBuf->restype < 290)
            rt = RTSHORT;
        else if (pBuf->restype < 330)
            rt = RTSTR;
        else if (pBuf->restype < 370)
            rt = RTENAME;
        else if (pBuf->restype < 999)
            rt = RT3DPOINT;
        else // pBuf->restype is already RTSHORT, RTSTR,
            rt = pBuf->restype; // etc. or it is unknown.

        switch (rt) {
        case RTSHORT:
            if (pBuf->restype == RTSHORT)
                acutPrintf(
				_T("短整类型数据（RTSHORT）为 : %d\n"), pBuf->resval.rint);
            else
                acutPrintf(_T("(%d . %d)\n"), pBuf->restype,
                    pBuf->resval.rint);
            break;
        case RTREAL:
            if (pBuf->restype == RTREAL)
                acutPrintf(
                _T("浮点类型数据（RTREAL）为  : %0.3f\n"), pBuf->resval.rreal);
            else
                acutPrintf(_T("(%d . %0.3f)\n"),
                    pBuf->restype, pBuf->resval.rreal);
            break;
        case RTSTR:
            if (pBuf->restype == RTSTR)
                acutPrintf(
                _T(" 字符类型数据（RTSTR）为 : %s\n"), pBuf->resval.rstring);
            else
                acutPrintf(_T("(%d . \"%s\")\n"),
                    pBuf->restype, pBuf->resval.rstring);
            break;
        case RT3DPOINT:
            if (pBuf->restype == RT3DPOINT)
                acutPrintf(
                _T("点类型数据（RT3DPOINT）为 : %0.3f, %0.3f, %0.3f\n"),
                    pBuf->resval.rpoint[X],
                    pBuf->resval.rpoint[Y],
                    pBuf->resval.rpoint[Z]);
            else
                acutPrintf(_T("(%d %0.3f %0.3f %0.3f)\n"),
                    pBuf->restype,
                    pBuf->resval.rpoint[X],
                    pBuf->resval.rpoint[Y],
                    pBuf->resval.rpoint[Z]);
            break;
        case RTLONG:
            acutPrintf(_T("长整类型数据（RTLONG）为 : %dl\n"), pBuf->resval.rlong);
            break;
        case -1:
        case RTENAME: // First block entity
            acutPrintf(_T("(%d . <实体名称: %8lx>)\n"),
                pBuf->restype, pBuf->resval.rlname[0]);
            break;
        case -3: // marks start of xdata
            acutPrintf(_T("(-3)\n"));
        }

        if ((i == 23) && (pBuf->rbnext != NULL)) {
            i = 0;
            acedGetString(0,
                _T("输入回车继续"), buf);
        }
    }
    return;
}

	//-------------------------------------------------------------------------------------------
	// 
	//  功能： 为数据库对象设置扩展数据
	//                 
	//
	//  作者： 
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH05.AddXdata command (do not rename)
	static void CGDCH05AddXdata(void)
	{
		//选择对象
		ads_name en;
		ads_point pt;
		acedEntSel( _T("\n选择实体: "), en, pt); 
		AcDbObjectId id;
		//转化ads_name为AcDbObjectId
		acdbGetObjectId(id,en);
		AcDbObject * pObj;
		//打开对象
		acdbOpenObject(pObj, id, AcDb::kForWrite );
		//输入应用程序名和添加到扩展数据的字符
		TCHAR appName[132], resString[200];
		appName[0] = resString[0] = _T('\0');
		acedGetString(NULL, _T("输入XDATA注册应用程序名: "),appName);
		acedGetString(NULL, _T("输入添加到扩展数据的字符串: "),resString);
		struct  resbuf  *pRb, *pTemp;
		pRb = pObj->xData(appName);
		if (pRb != NULL) {
			// 
			//如果已经有扩展数据了，将指针指向其尾端
			for (pTemp = pRb; pTemp->rbnext != NULL;
				pTemp = pTemp->rbnext)
			{ ; }
		} else {
			// 
			// 如果还没有扩展数据，则注册应用程序名
			acdbRegApp(appName);
			pRb = acutNewRb(AcDb::kDxfRegAppName);
			pTemp = pRb;
			pTemp->resval.rstring
				= (TCHAR*) malloc(_tcslen(appName) + 1);
			_tcscpy(pTemp->resval.rstring, appName);
		}
		// 
		//添加用户数据到resbuf链表
		pTemp->rbnext = acutNewRb(AcDb::kDxfXdAsciiString);
		pTemp = pTemp->rbnext;
		pTemp->resval.rstring
			= (TCHAR*) malloc(_tcslen(resString) + 1);
		_tcscpy(pTemp->resval.rstring, resString);
		// 
		//设置扩展数据
		pObj->upgradeOpen();
		pObj->setXData(pRb);
		pObj->close();
		acutRelRb(pRb);

	}
public:
   //-------------------------------------------------------------------------------------------
	// 
	//  功能： 获取数据库对象设置扩展数据
	//                 
	//
	//  作者： 
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH05.GetXdata command (do not rename)
	static void CGDCH05GetXdata(void)
	{
		//选择并打开实体
		ads_name en;
		ads_point pt;
		acedEntSel( _T("\n选择实体: "), en, pt); 
		AcDbObjectId id;
		//转化ads_name为AcDbObjectId
		acdbGetObjectId(id,en);
		AcDbObject * pObj;
		//打开对象
		acdbOpenObject(pObj, id, AcDb::kForWrite );
		//获得应用程序名
		TCHAR appname[133];
		if (acedGetString(NULL,_T("\n输入XDATA注册应用程序名称: "),
			appname) != RTNORM)
		{
			return;
		}
		// 
		//得到此应用程序的扩展数据列表
		struct resbuf *pRb;
		pRb = pObj->xData(appname);
		if (pRb != NULL) {

			//如果有此应用程序的扩展数据，可以操作其数据
			 printList(pRb);
			acutRelRb(pRb);
		} else {
			acutPrintf(_T("\n没有此应用程序的XDATA数据。"));
		}
		pObj->close();

	}
public:
//-------------------------------------------------------------------------------------------
	// 
	//  功能： 添加命名对象词典
	//                 
	//
	//  作者： 
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH05.CreateNodXrecord command (do not rename)
	static void CGDCH05CreateNodXrecord(void)
	{
		//得到命名对象词典指针
		AcDbDictionary *pNamedobj, *pDict;
		acdbHostApplicationServices()->workingDatabase()
			->getNamedObjectsDictionary(pNamedobj, AcDb::kForWrite);
		// 检查是否已有关键字为“设计信息”的词典，如没有，则创建
		if (pNamedobj->getAt(_T("设计信息"), (AcDbObject*&) pDict,
			AcDb::kForWrite) == Acad::eKeyNotFound)
		{
			pDict = new AcDbDictionary;
			AcDbObjectId DictId;
			pNamedobj->setAt(_T("设计信息"), pDict, DictId);
		}
		pNamedobj->close();
		//创建AcDbXrecord对象，并添加到扩展词典
		AcDbXrecord *pXrec = new AcDbXrecord;
		AcDbObjectId xrecObjId;
		pDict->setAt(_T("设计者"), pXrec, xrecObjId);
		pDict->close();
		//为AcDbXrecord对象创建resbuf链表
		struct resbuf *pHead;
		ads_point testpt = {1.0, 2.0, 0.0};
		pHead = acutBuildList(
			AcDb::kDxfText, _T(" "),   //姓名
			AcDb::kDxfText, _T("设计部门"),  //所在部门
			AcDb::kDxfReal, 5000.0,         //薪水
			AcDb::kDxfInt16, 4,             //工作年限
			0);
		// 为AcDbXrecord对象设置链表
		pXrec->setFromRbChain(*pHead);
		acutRelRb(pHead);
		pXrec->close();

	}
public:
//-------------------------------------------------------------------------------------------
	// 
	//  功能： 获取命名对象词典所包含的数据
	//                 
	//
	//  作者： 
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH05.GetNodXrecord command (do not rename)
	static void CGDCH05GetNodXrecord(void)
	{
		AcDbDictionary *pNamedobj;
		acdbHostApplicationServices()->workingDatabase()
			->getNamedObjectsDictionary(pNamedobj, AcDb::kForRead);
		//得到关键字为"设计信息"的词典
		AcDbDictionary *pDict;
		pNamedobj->getAt(_T("设计信息"), (AcDbObject*&)pDict,
			AcDb::kForRead);
		pNamedobj->close();

		//得到关键字为"设计者"的AcDbXrecord对象
		AcDbXrecord *pXrec;
		pDict->getAt(_T("设计者"), (AcDbObject*&) pXrec,
			AcDb::kForRead);
		pDict->close();
		struct resbuf *pRbList;
		pXrec->rbChain(&pRbList);
		pXrec->close();
		//返回的结果缓存的处理参考示例工程
		printListXRecord(pRbList);
		acutRelRb(pRbList);

	}
public:
    //-------------------------------------------------------------------------------------------
	// 
	//  功能： 为数据库对象创建扩展词典
	//                 
	//
	//  作者： 
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH05.CreateXrecord command (do not rename)
	static void CGDCH05CreateXrecord(void)
	{

		//选择并打开实体
		ads_name en;
		ads_point pt;
		acedEntSel( _T("\n选择实体: "), en, pt); 
		AcDbObjectId id;
		//转化ads_name为AcDbObjectId
		acdbGetObjectId(id,en);
		//打开对象
		AcDbObject * pObj;
		acdbOpenObject(pObj, id, AcDb::kForWrite );
		//创建扩展词典中要保存的对象
		AcDbXrecord *pXrec = new AcDbXrecord;
		//选择要添加扩展词典的对象
		AcDbObjectId dictObjId, xrecObjId;
		AcDbDictionary* pDict;
		//为选择的对象创建扩展词典，如已有，则什么都不作
		pObj->createExtensionDictionary();
		//得到此对象的扩展词典的ID 
		dictObjId = pObj->extensionDictionary();
		pObj->close();
		//打开扩展词典，并设置扩展词典记录
		acdbOpenObject(pDict, dictObjId, AcDb::kForWrite);
		pDict->setAt(_T("管道属性"), pXrec, xrecObjId);
		pDict->close();
		//创建AcDbXrecord所需的结果缓存
		struct resbuf* head;
		head = acutBuildList(
			AcDb::kDxfText, _T("TCH-0-8"), // 代号
			AcDb::kDxfText, _T("排水管-03"), // 名称
			AcDb::kDxfText, _T(" 水泥管"), // 材料
			AcDb::kDxfReal, 1.5,//管径
			AcDb::kDxfReal, 2.5,//埋深
			0);
		//为AcDbXrecord设置数据
		pXrec->setFromRbChain(*head);
		pXrec->close();
		acutRelRb(head);

	}
public:
//-------------------------------------------------------------------------------------------
	// 
	//  功能： 获取数据库对象的扩展词典数据
	//                 
	//
	//  作者： 
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH05.GetXrecord command (do not rename)
	static void CGDCH05GetXrecord(void)
	{
		//选择并打开实体
		ads_name en;
		ads_point pt;
		acedEntSel( _T("\n选择实体: "), en, pt); 
		AcDbObjectId id;
		//转化ads_name为AcDbObjectId
		acdbGetObjectId(id,en);
		//打开对象
		AcDbObject * pObj;
		acdbOpenObject(pObj, id, AcDb::kForWrite );

		//得到此对象的扩展词典的ID	
		AcDbObjectId dictObjId;
		dictObjId = pObj->extensionDictionary();
		if(dictObjId== AcDbObjectId::kNull)
		{
			acutPrintf(_T("\n选择的对象没有添加扩展词典。"));
			pObj->close();
			return;
		}
		pObj->close();
		//打开扩展词典，并获取扩展记录
		AcDbObjectId xrecObjId;
		AcDbDictionary* pDict;
		acdbOpenObject(pDict, dictObjId, AcDb::kForRead);

		//得到关键字为"管道属性"的AcDbXrecord对象
		AcDbXrecord *pXrec;
		pDict->getAt(_T("管道属性"), (AcDbObject*&) pXrec,	AcDb::kForRead);
		pDict->close();
		struct resbuf *pRbList;
		pXrec->rbChain(&pRbList);
		pXrec->close();
		//返回的结果缓存的处理
		printListXRecord(pRbList);
		acutRelRb(pRbList);
	}
} ;

//-----------------------------------------------------------------------------
IMPLEMENT_ARX_ENTRYPOINT(CCH05App)

ACED_ARXCOMMAND_ENTRY_AUTO(CCH05App, CGDCH05, AddXdata, AddXdata, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH05App, CGDCH05, GetXdata, GetXdata, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH05App, CGDCH05, CreateNodXrecord, CreateNodXrecord, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH05App, CGDCH05, GetNodXrecord, GetNodXrecord, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH05App, CGDCH05, CreateXrecord, CreateXrecord, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH05App, CGDCH05, GetXrecord, GetXrecord, ACRX_CMD_TRANSPARENT, NULL)
