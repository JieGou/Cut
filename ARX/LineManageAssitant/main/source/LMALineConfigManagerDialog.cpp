#include "stdafx.h"

#include "LMALineConfigManagerDialog.h"

#include "AddLineDialog.h"

LMALineConfigManagerDialog::LMALineConfigManagerDialog(CWnd* parent)
	:CAcUiDialog(LMALineConfigManagerDialog::IDD, parent)
{
	acutPrintf(L"初始化管线类型配置数据.\n");
	init();
}

LMALineConfigManagerDialog::~LMALineConfigManagerDialog(void)
{
}

void LMALineConfigManagerDialog::DoDataExchange(CDataExchange* pDX)
{
	CAcUiDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_LINE_CONFIG, m_LineList);
}

void LMALineConfigManagerDialog::init()
{
	initLineHeader();
	initLineData();
}

void LMALineConfigManagerDialog::initLineHeader()
{
	acutPrintf(L"初始化管线数据抬头.\n");
	int index = 0;
	m_LineList.InsertColumn(index++,L"类型");
	m_LineList.InsertColumn(index++,L"名称");
	m_LineList.InsertColumn(index++,L"断面形状");
	m_LineList.InsertColumn(index++,L"断面大小");
	m_LineList.InsertColumn(index++,L"描述");
	m_LineList.InsertColumn(index++,L"单位");
	m_LineList.InsertColumn(index++,L"有效范围");
}

void LMALineConfigManagerDialog::initLineData()
{
}

//void LMALineConfigManagerDialog::OnBnClickedButtonAdd()
//{
//	// TODO: Add your control notification handler code here
//	AddLineDialog dlg(this);
//	INT_PTR nReturnValue = dlg.DoModal();
//}


BEGIN_MESSAGE_MAP(LMALineConfigManagerDialog, CAcUiDialog)
//	ON_BN_CLICKED(IDC_BUTTON_ADD, &LMALineConfigManagerDialog::OnBnClickedButtonAdd)
END_MESSAGE_MAP()
