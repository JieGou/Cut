#pragma once

#include <resource.h>
#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
//#include <acui.h>
//#include <adui.h>

#include <string>
using namespace std;

// AddLineConfigDialog dialog

namespace com
{

namespace guch
{

namespace assistent
{

namespace config
{

typedef struct _LineConfigData
{
	int mIndex;
	UINT mID;
	wstring mName;
	wstring mKind;
	wstring mCategory;
	wstring mShape;
	wstring mSize;
	wstring mSafeSize;
	wstring mUnit;
	wstring mDesc;
} LineConfigData;

class AddLineConfigDialog : public CDialog
{
	DECLARE_DYNAMIC(AddLineConfigDialog)

public:
	AddLineConfigDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~AddLineConfigDialog();

// Dialog Data
	enum { IDD = IDD_DIALOG_LINE_CONFIG_ADD };

	typedef enum { OPER_ADD, OPER_UPDATE} OPER_TYPE;

	void SetOperType( OPER_TYPE type ){ m_OperType = type; };
	OPER_TYPE GetOperType() const { return m_OperType;}

	void fillData(LineConfigData& configData);

protected:

	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	BOOL InitConfigData();

	DECLARE_MESSAGE_MAP()

private:

	CComboBox m_LineKind;
	CComboBox m_LineShape;
	CComboBox m_LineUnit;

	CEdit m_LineName;
	CEdit m_LineSize;
	CEdit m_LineSafeSize;
	CEdit m_LineDesc;

	OPER_TYPE m_OperType;
	int m_lineIndex;

	afx_msg void OnBnClickedOk();
};

// AddLineDialog dialog

class AddLineDialog : public CDialog
{
	DECLARE_DYNAMIC(AddLineDialog)

public:
	AddLineDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~AddLineDialog();

// Dialog Data
	enum { IDD = IDD_DIALOG_LINE_ADD };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	BOOL InitLineList();
	BOOL InitLineDetailHeader();
	BOOL InitLineDetailData();

	DECLARE_MESSAGE_MAP()

public:
	//CTreeCtrl m_LinesTree;
	//CListCtrl m_LineDetailList;
};

} // end of config

} // end of assistant

} // end of guch

} // end of com
