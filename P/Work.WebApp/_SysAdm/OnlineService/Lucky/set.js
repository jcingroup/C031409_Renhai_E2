function check(f)
{
	var strMsg = ""
	var intFlag = 0

	// if(f.p1.value=="")
	// {	strMsg+="「帳號」"
	//    intFlag+=1}

	// if(f.p1.value=="")
	// {	strMsg+="「姓名」"
	//    intFlag+=1}

	if(intFlag != 0)
	{
		alert("請填寫" + strMsg + "欄位，謝謝！")
		return false
	}
}
