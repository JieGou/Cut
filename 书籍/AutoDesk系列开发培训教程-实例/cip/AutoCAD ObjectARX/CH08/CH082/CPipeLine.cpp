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
//----- CPipeLine.cpp : Implementation of CPipeLine
//-----------------------------------------------------------------------------
#include "StdAfx.h"
#include "CPipeLine.h"
#include <tchar.h>
//-----------------------------------------------------------------------------
Adesk::UInt32 CPipeLine::kCurrentVersionNumber =1 ;

//-----------------------------------------------------------------------------
ACRX_DXF_DEFINE_MEMBERS (
	CPipeLine, AcDbPolyline,
	AcDb::kDHL_CURRENT, AcDb::kMReleaseCurrent, 
	AcDbProxyEntity::kNoOperation, PIPELINE,
	"CSCH082APP"
	"|Product Desc:     A description for your object"
	"|Company:          Your company name"
	"|WEB Address:      Your company WEB site address"
)

//-----------------------------------------------------------------------------
CPipeLine::CPipeLine () : AcDbPolyline () {
}

CPipeLine::~CPipeLine () {
}

//-----------------------------------------------------------------------------
//----- AcDbObject protocols
//- Dwg Filing protocol
Acad::ErrorStatus CPipeLine::dwgOutFields (AcDbDwgFiler *pFiler) const {
	assertReadEnabled () ;
	//----- Save parent class information first.
	Acad::ErrorStatus es =AcDbPolyline::dwgOutFields (pFiler) ;
	if ( es != Acad::eOk )
		return (es) ;
	//----- Object version number needs to be saved first
	if ( (es =pFiler->writeUInt32 (CPipeLine::kCurrentVersionNumber)) != Acad::eOk )
		return (es) ;
	///写入数据开始
	pFiler->writeItem(m_dRadius);
	pFiler->writeItem(m_dThickness);
	pFiler->writeItem(m_dDeep);
	pFiler->writeString(m_cMaterial);
	//写入数据结束

	return (pFiler->filerStatus ()) ;
}

Acad::ErrorStatus CPipeLine::dwgInFields (AcDbDwgFiler *pFiler) {
	assertWriteEnabled () ;
	//----- Read parent class information first.
	Acad::ErrorStatus es =AcDbPolyline::dwgInFields (pFiler) ;
	if ( es != Acad::eOk )
		return (es) ;
	//----- Object version number needs to be read first
	Adesk::UInt32 version =0 ;
	if ( (es =pFiler->readUInt32 (&version)) != Acad::eOk )
		return (es) ;
	if ( version > CPipeLine::kCurrentVersionNumber )
		return (Acad::eMakeMeProxy) ;
	 //读取数据开始
		pFiler->readItem(&m_dRadius);
		pFiler->readItem(&m_dThickness);
		pFiler->readItem(&m_dDeep);
		TCHAR *pString=NULL;
		pFiler->readString(&pString);
		_tcscpy(m_cMaterial,pString);
	// 读取数据结束

	return (pFiler->filerStatus ()) ;
}

//- Dxf Filing protocol
Acad::ErrorStatus CPipeLine::dxfOutFields (AcDbDxfFiler *pFiler) const {
	assertReadEnabled () ;
	//----- Save parent class information first.
	Acad::ErrorStatus es =AcDbPolyline::dxfOutFields (pFiler) ;
	if ( es != Acad::eOk )
		return (es) ;
	es =pFiler->writeItem (AcDb::kDxfSubclass, _RXST("CPipeLine")) ;
	if ( es != Acad::eOk )
		return (es) ;
	//----- Object version number needs to be saved first
	if ( (es =pFiler->writeUInt32 (kDxfInt32, CPipeLine::kCurrentVersionNumber)) != Acad::eOk )
		return (es) ;
		return (es) ;
	////写入数据开始
	pFiler->writeItem(AcDb::kDxfReal , m_dRadius);
	pFiler->writeItem(AcDb::kDxfReal+1 , m_dThickness);
	pFiler->writeItem(AcDb::kDxfReal+2 , m_dDeep);
	pFiler->writeItem(AcDb::kDxfText ,m_cMaterial);
	////写入数据结束
	return (pFiler->filerStatus ()) ;
}

Acad::ErrorStatus CPipeLine::dxfInFields (AcDbDxfFiler *pFiler) {
	assertWriteEnabled () ;
	//----- Read parent class information first.
	Acad::ErrorStatus es =AcDbPolyline::dxfInFields (pFiler) ;
	if ( es != Acad::eOk || !pFiler->atSubclassData (_RXST("CPipeLine")) )
		return (pFiler->filerStatus ()) ;
	//----- Object version number needs to be read first
	struct resbuf rb ;
	pFiler->readItem (&rb) ;
	if ( rb.restype != AcDb::kDxfInt32 ) {
		pFiler->pushBackItem () ;
		pFiler->setError (Acad::eInvalidDxfCode, _RXST("\nError: expected group code %d (version #)"), AcDb::kDxfInt32) ;
		return (pFiler->filerStatus ()) ;
	}
	Adesk::UInt32 version =(Adesk::UInt32)rb.resval.rlong ;
	if ( version > CPipeLine::kCurrentVersionNumber )
		return (Acad::eMakeMeProxy) ;
	//- Uncomment the 2 following lines if your current object implementation cannot
	//- support previous version of that object.
	//if ( version < CPipeLine::kCurrentVersionNumber )
	//	return (Acad::eMakeMeProxy) ;
	//----- Read params in non order dependant manner
	while ( es == Acad::eOk && (es =pFiler->readResBuf (&rb)) == Acad::eOk ) {
		switch ( rb.restype ) {
			// 从DXF中读取数据
			case AcDb::kDxfReal:
					m_dRadius =rb.resval.rreal ;
					break ;
			case AcDb::kDxfReal+1:
					m_dThickness =rb.resval.rreal ;
					break ;
			case AcDb::kDxfReal+2:
					m_dDeep =rb.resval.rreal ;
					break ;
			case AcDb::kDxfText:
					_tcscpy(m_cMaterial,rb.resval.rstring);
					break ;
			default:
				//
				pFiler->pushBackItem () ;
				es =Acad::eEndOfFile ;
				break ;
		}
	}
	//----- At this point the es variable must contain eEndOfFile
	//----- - either from readResBuf() or from pushback. If not,
	//----- it indicates that an error happened and we should
	//----- return immediately.
	if ( es != Acad::eEndOfFile )
		return (Acad::eInvalidResBuf) ;

	return (pFiler->filerStatus ()) ;
}

//-----------------------------------------------------------------------------
//----- AcDbEntity protocols
Adesk::Boolean CPipeLine::worldDraw (AcGiWorldDraw *mode) {
	assertReadEnabled () ;
	// 准备数据
	int nVerts = AcDbPolyline::numVerts();
	AcGePoint3d pt;
	AcGeVector3d norm = AcDbPolyline::normal ();
	//AcGeVector3d dir(1.0, 0.0, 0.0);
	// 设定绘制颜色为青色
	mode->subEntityTraits().setColor(4);
	// 
	double	 m_dRadius = 120.0;
	double m_dThickness = 10.0;
	double m_dDeep = 2.4;
 	TCHAR m_cMaterial[128];
   _tcscpy(m_cMaterial,_T("水泥管"));

	 // 计算方向向量
	 AcDbPolyline::getPointAt(0,pt);
	 AcGePoint3d pt1;
	 AcDbPolyline::getPointAt(1,pt1);
	AcGeVector3d vec = pt1-pt;
	vec.normalize();
	// 计算垂直向量
	AcGeVector3d vecV;
	vecV = vec.crossProduct (AcGeVector3d::kZAxis);
	// 绘制管径标签
	TCHAR buf[100];
	pt1 = pt + vecV*2.0;
     _stprintf(buf,_T( " 管径：%4.2f"), m_dRadius);
	mode->geometry ().text (pt1,norm,vec,1.0,1.0,0,buf);
	// 绘制壁厚标签
	pt1 = pt1 + vecV*2.0;
     _stprintf(buf,_T( " 壁厚：%4.2f"), m_dThickness);
	mode->geometry ().text (pt1,norm,vec,1.0,1.0,0,buf);
	// 绘制埋深标签
	pt1 = pt1 + vecV*2.0;
     _stprintf(buf,_T( " 埋深：%4.2f"), m_dDeep);
	mode->geometry ().text (pt1,norm,vec,1.0,1.0,0,buf);
	// 绘制材质标签
	pt1 = pt1 + vecV*2.0;
     _stprintf(buf,_T( " 材质：%s"), m_cMaterial);
	mode->geometry ().text (pt1,norm,vec,1.0,1.0,0,buf);
	// 设定颜色为红色
	mode->subEntityTraits().setColor(1);

	for(int i = 0;i<nVerts;i++)
	{
		AcDbPolyline::getPointAt (i,pt);
		mode->geometry ().circle (pt,1.0,norm);
	}
	// 设定颜色为黄色
	mode->subEntityTraits().setColor(3);
   // 
	return (AcDbPolyline::worldDraw (mode)) ;
}


