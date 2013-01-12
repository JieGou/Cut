#include <ArxWrapper.h>
#include <GlobalDataConfig.h>
#include <LineCategoryItemData.h>
#include <LineConfigDataManager.h>

#include "acedads.h"
#include "accmd.h"
#include <adscodes.h>

#include <adsdlg.h>

#include <dbapserv.h>

#include <dbregion.h>

#include <gepnt3d.h>

//symbol table
#include <dbsymtb.h>

//3D Object
#include <dbsol3d.h>

// Custom object
#include <ArxCustomObject.h>

using namespace com::guch::assistant::arx;
using namespace com::guch::assistant::data;
using namespace com::guch::assistant::config;

/**
 * 将对象放置在模型空间中的某一层上
 **/
AcDbObjectId ArxWrapper::PostToModelSpace(AcDbEntity* pEnt,const wstring& layerName )
{
	if( !pEnt )
		return 0;

	//打开块表数据库
	AcDbBlockTable *pBlockTable;
	acdbHostApplicationServices()->workingDatabase()
		->getBlockTable(pBlockTable, AcDb::kForRead);

	//得到模型空间
	AcDbBlockTableRecord *pBlockTableRecord;
	pBlockTable->getAt(ACDB_MODEL_SPACE, pBlockTableRecord,
	AcDb::kForWrite);

	//加入实体
	AcDbObjectId entId;
	pBlockTableRecord->appendAcDbEntity(entId, pEnt);
	pEnt->setLayer(layerName.c_str());

	//关闭实体
	pBlockTable->close();
	pBlockTableRecord->close();
	pEnt->close();

	return entId;
}

/**
 * 创建特定名称的层
 **/
void ArxWrapper::createNewLayer(const wstring& layerName)
{
	//打开层表数据库
	AcDbLayerTable *pLayerTable;
    acdbHostApplicationServices()->workingDatabase()
        ->getSymbolTable(pLayerTable, AcDb::kForWrite);

	//层表不存在，则创建
	if( pLayerTable )
	{
		AcDbLayerTableRecord *pLayerTableRecord = NULL;

		if(pLayerTable->has(layerName.c_str()))
		{
			//构建新的层表记录
			AcDbLayerTableRecord *pLayerTableRecord =
				new AcDbLayerTableRecord;
			pLayerTableRecord->setName(layerName.c_str());

			// Defaults are used for other properties of 
			// the layer if they are not otherwise specified.
			pLayerTable->add(pLayerTableRecord);
		}

		pLayerTable->close();

		if( pLayerTableRecord )
			pLayerTableRecord->close();
	}
}

/**
 * 根据起始点创建线段，并放置在特定的层上
 **/
AcDbObjectId ArxWrapper::createLine( const AcGePoint3d& start,
							const AcGePoint3d& end,
							const wstring& layerName )
{
    AcDbLine *pLine = new AcDbLine(start, end);
    return ArxWrapper::PostToModelSpace(pLine,layerName);
}

/**
 * 根据起始点队列（向量列表），并放置在特定的层上
 **/
void ArxWrapper::createLine( const Point3dVector& points3d,
							const wstring& layerName )
{
	if( points3d.size() < 2 )
		return;

	AcGePoint3d *pStart = NULL;

	for( Point3dIter iter = points3d.begin();
		iter != points3d.end();
		iter++)
	{
		if( pStart == NULL )
		{
			pStart = *iter;
			continue;
		}
		else
		{
			createLine( *pStart, *(*iter), layerName );
			pStart = *iter;
		}
	}
}

/**
 * 根据管线实体起始点队列（向量列表），并放置在特定的层上
 **/
void ArxWrapper::createLine( const PointList& points,
							const wstring& layerName )
{
	if( points.size() < 2 )
		return;

	AcGePoint3d *pStart = NULL;

	for( ContstPointIter iter = points.begin();
		iter != points.end();
		iter++)
	{
		if( pStart == NULL )
		{
			pStart = new AcGePoint3d((*iter)->m_Point[X],
										(*iter)->m_Point[Y],
										(*iter)->m_Point[Z]);
			continue;
		}
		else
		{
			AcGePoint3d *pEnd = new AcGePoint3d((*iter)->m_Point[X],
										(*iter)->m_Point[Y],
										(*iter)->m_Point[Z]);

			createLine( *pStart, *pEnd, layerName );

			delete pStart;

			pStart = pEnd;
		}
	}

	if( pStart != NULL )
		delete pStart;
}

/**
 * 根据起始点队列（向量列表），并放置在特定的层上
 **/
AcDbEntity* ArxWrapper::MoveToBottom(AcDbEntity* pEntry)
{
	if( !pEntry )
		return NULL;

	AcGeVector3d vec(-8,10,0);

	AcGeMatrix3d moveMatrix;
	moveMatrix.setToTranslation(vec);

	pEntry->transformBy(moveMatrix);

	return pEntry;
}

/**
 * 根据起始点队列（向量列表），并放置在特定的层上
 **/
AcDb3dSolid* ArxWrapper::DrawCylinder(const UINT& lineID,
										const UINT& sequenceID,
										const AcGePoint3d& start,
										const AcGePoint3d& end,
										const wstring& layerName,
										const double& radius )
{
#ifdef DEBUG
	acutPrintf(L"绘制圆柱体实例，加入到图层空间\n");
#endif

	LMALineDbObject* lmaLineObj = new LMALineDbObject(Adesk::Int16(lineID),
															Adesk::Int16(sequenceID),
															start,end,radius,NULL);
	PostToModelSpace(lmaLineObj,layerName);

	return lmaLineObj;
}

/**
 * 根据导入线段配置，创建多线段3D折线
 **/
void ArxWrapper::createLMALine( const LineEntry& lineEntry )
{
#ifdef DEBUG
	acutPrintf(L"在图层【%s】绘制管线实体，断点个数【%d】\n",lineEntry.m_LineName.c_str(),lineEntry.m_PointList->size());
#endif

	try
	{
		//得到实体的属性
		LineCategoryItemData* lineCategory = LineConfigDataManager::Instance()->FindByKind(lineEntry.m_LineKind);
		if( !lineCategory )
		{
			acutPrintf(L"管线【%s】编号【%s】类型【%s】没有配置数据，不能创建3D模型。\n",lineEntry.m_LineName.c_str(),lineEntry.m_LineNO.c_str(),lineEntry.m_LineKind.c_str());
			return;
		}
		
		//首先创建图层
		createNewLayer(lineEntry.m_LineName);

		//如果是圆柱
		if( lineCategory->mShape == GlobalData::LINE_SHAPE_CIRCLE )
		{
			//得到半径
			const wstring& rRadius = lineCategory->mRadius;

			double radius = _wtol(rRadius.c_str());

			//然后创建柱体
#ifdef DEBUG
			acutPrintf(L"绘制圆柱体，半径为【%lf】\n",radius);
#endif
			DrawPolyCylinder(lineEntry,radius);
		}
		else if ( lineCategory->mShape == GlobalData::LINE_SHAPE_SQUARE )
		{
			//得到长宽

			//创建方柱体
		}
	}
	catch(...)
	{
	}
}

/**
 * 创建多线段3D折线
 **/
void ArxWrapper::DrawPolyCylinder( const LineEntry& lineEntry, const double& radius)
{
	const PointList& points = *(lineEntry.m_PointList);
	const wstring& layerName = lineEntry.m_LineName;

	if( points.size() < 2 )
		return;

	AcGePoint3d *pStart = NULL;

	for( ContstPointIter iter = points.begin();
		iter != points.end();
		iter++)
	{
		if( pStart == NULL )
		{
			//多线段的第一个起点
			pStart = new AcGePoint3d((*iter)->m_Point[X],
										(*iter)->m_Point[Y],
										(*iter)->m_Point[Z]);
			continue;
		}
		else
		{
			AcGePoint3d *pEnd = new AcGePoint3d((*iter)->m_Point[X],
										(*iter)->m_Point[Y],
										(*iter)->m_Point[Z]);

			//创建3D柱体代表直线
			DrawCylinder( lineEntry.m_LineID, (*iter)->m_PointNO, *pStart, *pEnd, layerName,radius );

			delete pStart;

			//继续下一个线段
			pStart = pEnd;
		}
	}

	if( pStart != NULL )
		delete pStart;
}
