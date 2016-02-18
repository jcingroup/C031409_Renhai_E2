
function check(f)
{	var strMsg,intFlag
   strMsg = ""
   intFlag = 0
   
   if (isNaN(f.p7.value)) 
   {   
	  alert("地坪請填入數字");   
      return false;   
   }   
   

   if (isNaN(f.p9.value)) 
   {   
	  alert("面寬M請填入數字");   
      return false;   
   }   
   
   if (isNaN(f.p10.value)) 
   {   
	  alert("深度M請填入數字");   
      return false;   
   }   
   
   if (isNaN(f.p11.value)) 
   {   
	  alert("臨路M請填入數字");   
      return false;   
   }   
   
    if (isNaN(f.p101.value)) 
   {   
	  alert("第二個面寬M請填入數字");   
      return false;   
   }   
   
   if (isNaN(f.p102.value)) 
   {   
	  alert("第二個深度M請填入數字");   
      return false;   
   }   
   
   if (isNaN(f.p103.value)) 
   {   
	  alert("第二個臨路M請填入數字");   
      return false;   
   }   
   
   if (isNaN(f.p13.value)) 
   {   
	  alert("每坪單價請填入數字");   
      return false;   
   }   
   
   if (isNaN(f.p14.value)) 
   {   
	  alert("總價請填入數字");   
      return false;   
   }   
   
   
   if(f.p1.value =="")
   {	strMsg+="「歸檔編號」"
      intFlag+=1}

	if(f.p2.value =="")
   {	strMsg+="「物件名稱」"
      intFlag+=1}
	
	if(f.p13.value =="")
   {	strMsg+="「每坪單價」"
      intFlag+=1}
   
	if(f.p14.value =="")
   {	strMsg+="「總價」"
      intFlag+=1}
	
  
   if(intFlag!=0)
    {  alert("請填寫"+strMsg+"欄位，謝謝！")
		return false}

}


