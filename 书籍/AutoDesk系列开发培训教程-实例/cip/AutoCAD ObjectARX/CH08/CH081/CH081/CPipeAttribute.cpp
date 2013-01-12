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
//----- CPipeAttribute.cpp : Implementation of CPipeAttribute
//-----------------------------------------------------------------------------
#include "StdAfx.h"
#include <tchar.h>
#include "CPipeAttribute.h"

//-----------------------------------------------------------------------------
Adesk::UInt32 CPipeAttribute::kCurrentVersionNumber =1 ;

//-----------------------------------------------------------------------------
ACRX_DXF_DEFINE_MEMBERS (
	CPipeAttribute, AcDbObject,
	AcDb::kDHL_CURRENT, AcDb::kMReleaseCurrent, 
	AcDbProxyEntity::kNoOperation, PIPEATTRIBUTE,
	"CSCH081APP"
	"|Product Desc:     A description for your object"
	"|Company:          Your company name"
	"|WEB Address:      Your company WEB site address"
)

//-----------------------------------------------------------------------------
CPipeAttribute::CPipeAttribute () : AcDbObject () {
	 m_dRadius = 120.0;
	 m_dThickness = 10.0;
	 m_dDeep = 2.4;
	_tcscpy(m_cMaterial,_T("水泥管"));

}

CPipeAttribute::~CPipeAttribute () {
}

//-----------------------------------------------------------------------------
//----- AcDbObject protocols
//- Dwg Filing protocol
Acad::ErrorStatus CPipeAttribute::dwgOutFields (AcDbDwgFiler *pFiler) const {
	//
	assertReadEnabled () ;
	//----- Save parent class information first.
	Acad::ErrorStatus es =AcDbObject::dwgOutFields (pFiler) ;
	if ( es != Acad::eOk )
		return (es) ;
	//----- Object version number needs to be saved first
	if ( (es =pFiler->writeUInt32 (CPipeAttribute::kCurrentVersionNumber)) != Acad::eOk )
		return (es) ;
	///写入数据开始
	pFiler->writeItem(m_dRadius);
	pFiler->writeItem(m_dThickness);
	pFiler->writeItem(m_dDeep);
	pFiler->writeString(m_cMaterial);
	//写入数据结束
	return (pFiler->filerStatus ()) ;
}

Acad::ErrorStatus CPipeAttribute::dwgInFields (AcDbDwgFiler *pFiler) {
	assertWriteEnabled () ;
	//----- Read parent class information first.
	Acad::ErrorStatus es =AcDbObject::dwgInFields (pFiler) ;
	if ( es != Acad::eOk )
		return (es) ;
	//----- Object version number needs to be read first
	Adesk::UInt32 version =0 ;
	if ( (es =pFiler->readUInt32 (&version)) != Acad::eOk )
		return (es) ;
	if ( version > CPipeAttribute::kCurrentVersionNumber )
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
Acad::ErrorStatus CPipeAttribute::dxfOutFields (AcDbDxfFiler *pFiler) const {
	// 检查对象处于正确的打开状态
	assertReadEnabled () ;
	//父类数据的重载
	Acad::ErrorStatus es =AcDbObject::dxfOutFields (pFiler) ;
	if ( es != Acad::eOk )
		return (es) ;
	es =pFiler->writeItem (AcDb::kDxfSubclass, _RXST("CPipeAttribute")) ;
	if ( es != Acad::eOk )
		return (es) ;
	//----- Object version number needs to be saved first
	if ( (es =pFiler->writeUInt32 (kDxfInt32, CPipeAttribute::kCurrentVersionNumber)) != Acad::eOk )
		return (es) ;
	////写入数据开始
	pFiler->writeItem(AcDb::kDxfReal , m_dRadius);
	pFiler->writeItem(AcDb::kDxfReal+1 , m_dThickness);
	pFiler->writeItem(AcDb::kDxfReal+2 , m_dDeep);
	pFiler->writeItem(AcDb::kDxfText ,m_cMaterial);
	////写入数据结束

	return (pFiler->filerStatus ()) ;
}

Acad::ErrorStatus CPipeAttribute::dxfInFields (AcDbDxfFiler *pFiler) {
	assertWriteEnabled () ;
	//----- Read parent class information first.
	Acad::ErrorStatus es =AcDbObject::dxfInFields (pFiler) ;
	if ( es != Acad::eOk || !pFiler->atSubclassData (_RXST("CPipeAttribute")) )
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
	if ( version > CPipeAttribute::kCurrentVersionNumber )
		return (Acad::eMakeMeProxy) ;
	
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

	if ( es != Acad::eEndOfFile )
		return (Acad::eInvalidResBuf) ;

	return (pFiler->filerStatus ()) ;
}

