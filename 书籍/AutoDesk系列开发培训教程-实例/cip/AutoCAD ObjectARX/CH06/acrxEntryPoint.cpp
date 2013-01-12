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
#include <tchar.h>
#define PI 3.14159265358
//-----------------------------------------------------------------------------
#define szRDS _RXST("CGD")

//-----------------------------------------------------------------------------
//----- ObjectARX EntryPoint
class CCH06App : public AcRxArxApp {

public:
	CCH06App () : AcRxArxApp () {}

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
	//  功能： 第二条直线的起点移动到第一条直线的中点位置，
	//                 并通过构造旋转矩阵使得第二条直线和第一条直线垂直
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH06.TransformLine command (do not rename)
	static void CGDCH06TransformLine(void)
	{


		ads_name      ename1,ename2;
		ads_point     pickpt;
		if (acedEntSel(_T("\n选择直线1: "), ename1, pickpt)  != RTNORM)
		{
			acutPrintf(_T("\n选择直线1失败"));
			return ;
		}
		if (acedEntSel(_T("\n选择直线2: "), ename2, pickpt)  != RTNORM)
		{
			acutPrintf(_T("\n选择直线2失败"));
			return ;
		}	

		AcDbObjectId IdLine1,IdLine2;

		acdbGetObjectId(IdLine1, ename1);		
		acdbGetObjectId(IdLine2, ename2);
		//直线对象
		AcDbLine *pLine1 = NULL;			
		AcDbLine *pLine2 = NULL;	
		//以读方式打开实体
		if(Acad::eOk == acdbOpenObject(pLine1, IdLine1, AcDb::kForRead))
		{
			if(Acad::eOk == acdbOpenObject(pLine2, IdLine2, AcDb::kForRead))
			{

				// 计算第一条直线的中点
				AcGePoint3d ptMid; 
				ptMid.x =  0.5*(pLine1->endPoint ().x + pLine1->startPoint ().x);
				ptMid.y=  0.5*(pLine1->endPoint ().y + pLine1->startPoint ().y);
				ptMid.z =  0.5*(pLine1->endPoint ().z + pLine1->startPoint ().z);	

				// 第二条直线起点到第一条直线的向量
				AcGeVector3d vecMove = ptMid -  pLine2->startPoint ();
				// 构造平移矩阵
				AcGeMatrix3d  mat,mat1;
				mat = mat.translation (vecMove);
				//  修改第二条直线
				pLine2->upgradeOpen ();
				pLine2->transformBy (mat);
				//计算两条直线的夹角
				AcGeVector3d  vec1 = pLine1->endPoint ()- pLine1->startPoint ();
				AcGeVector3d  vec2 = pLine2->endPoint ()- pLine2->startPoint ();
				double dAng;
				dAng = vec1.angleTo(vec2);
				AcGeVector3d vecZ(0.0,0.0,1.0);
				//计算旋转角度
				double dRot;
				if(dAng<0.5*PI)
				{
					dRot = 0.5*PI  - dAng;
				}
				else
				{
					dRot =dAng  -  0.5*PI ;
				}
				// 构造旋转矩阵
				mat1  = mat1.rotation (dRot,vecZ,pLine2->startPoint ());
				//对直线进行旋转变换
				pLine2->transformBy (mat1);
				pLine2->setColorIndex (1);
				//关闭实体
				pLine2->close();
				acutPrintf(_T("\n 操作完成 。"));

			}
			//关闭实体
			pLine1->close();	

		}


		// Add your code for command CGDCH06.TransformLine here
	}
public:
    //-------------------------------------------------------------------------------------------
	// 
	//  功能： 求直线的交点和夹角
	//
	//  作者：Zhao C.X.
	//
	// 日期：200709
	//
	//  历史：
	//          调整部分代码 BY	Qin H.X.
	//
	//----------------------------------------------------------------------------------------------
	// - CGDCH06.GetIntersect command (do not rename)
	static void CGDCH06GetIntersect(void)
	{

	ads_name      ename1,ename2;
    ads_point     pickpt;
    if (acedEntSel(_T("\n选择直线1: "), ename1, pickpt)  != RTNORM)
    {
        acutPrintf(_T("\n选择直线1失败"));
        return ;
    }
    if (acedEntSel(_T("\n选择直线2: "), ename2, pickpt)  != RTNORM)
    {
        acutPrintf(_T("\n选择直线2失败"));
        return ;
    }	
	
	AcDbObjectId IdLine1,IdLine2;

	acdbGetObjectId(IdLine1, ename1);		
	acdbGetObjectId(IdLine2, ename2);
	//直线对象
	AcDbLine *pLine1 = NULL;			
	AcDbLine *pLine2 = NULL;	
	//以读方式打开实体
	if(Acad::eOk == acdbOpenObject(pLine1, IdLine1, AcDb::kForRead))
	{
		if(Acad::eOk == acdbOpenObject(pLine2, IdLine2, AcDb::kForRead))
		{
			//创建几何直线
			AcGeLineSeg3d geLineSeg1, geLineSeg2;
			geLineSeg1.set(pLine1->startPoint(), pLine1->endPoint());	
			geLineSeg2.set(pLine2->startPoint(), pLine2->endPoint());

			//求直线的交点
			AcGePoint3d ptIntersect;	
			//如果两条直线有交点
			if (geLineSeg1.intersectWith(geLineSeg2, ptIntersect))	
			{
				acutPrintf(_T("\n两直线的交点：(%0.2f, %0.2f, %0.2f)"), ptIntersect.x, ptIntersect.y,ptIntersect.z);
			}
			else
			{
				acutPrintf(_T("\n直线没有交点。"));
			}

			///求直线的夹角
			AcGeVector3d vect1, vect2;	
			vect1 = geLineSeg1.direction();
			vect2 = geLineSeg2.direction();
			double dAngle;
			dAngle = vect1.angleTo(vect2);
			dAngle *= 180.0 / PI;
			pLine2->close();
			acutPrintf(_T("\n直线的夹角为：%0.0f度"), dAngle);

		}
			//关闭实体
		pLine1->close();	

	}

	}
} ;

//-----------------------------------------------------------------------------
IMPLEMENT_ARX_ENTRYPOINT(CCH06App)

ACED_ARXCOMMAND_ENTRY_AUTO(CCH06App, CGDCH06, TransformLine, TransformLine, ACRX_CMD_TRANSPARENT, NULL)
ACED_ARXCOMMAND_ENTRY_AUTO(CCH06App, CGDCH06, GetIntersect, GetIntersect, ACRX_CMD_TRANSPARENT, NULL)
