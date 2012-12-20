// AddLineDialog.cpp : implementation file
//

#include "stdafx.h"
#include "AddLineDialog.h"
#include "afxdialogex.h"


// AddLineDialog dialog

IMPLEMENT_DYNAMIC(AddLineDialog, CDialogEx)

AddLineDialog::AddLineDialog(CWnd* pParent /*=NULL*/)
	: CAcUiDialog(AddLineDialog::IDD, pParent)
{

}

AddLineDialog::~AddLineDialog()
{
}

void AddLineDialog::DoDataExchange(CDataExchange* pDX)
{
	CAcUiDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(AddLineDialog, CAcUiDialog)
END_MESSAGE_MAP()


// AddLineDialog message handlers
