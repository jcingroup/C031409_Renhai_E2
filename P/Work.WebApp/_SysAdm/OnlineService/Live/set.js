
function check(f)
{	var strMsg,intFlag
   strMsg = ""
   intFlag = 0
   
   if (isNaN(f.p7.value)) 
   {   
	  alert("�a�W�ж�J�Ʀr");   
      return false;   
   }   
   

   if (isNaN(f.p9.value)) 
   {   
	  alert("���eM�ж�J�Ʀr");   
      return false;   
   }   
   
   if (isNaN(f.p10.value)) 
   {   
	  alert("�`��M�ж�J�Ʀr");   
      return false;   
   }   
   
   if (isNaN(f.p11.value)) 
   {   
	  alert("�{��M�ж�J�Ʀr");   
      return false;   
   }   
   
    if (isNaN(f.p101.value)) 
   {   
	  alert("�ĤG�ӭ��eM�ж�J�Ʀr");   
      return false;   
   }   
   
   if (isNaN(f.p102.value)) 
   {   
	  alert("�ĤG�Ӳ`��M�ж�J�Ʀr");   
      return false;   
   }   
   
   if (isNaN(f.p103.value)) 
   {   
	  alert("�ĤG���{��M�ж�J�Ʀr");   
      return false;   
   }   
   
   if (isNaN(f.p13.value)) 
   {   
	  alert("�C�W����ж�J�Ʀr");   
      return false;   
   }   
   
   if (isNaN(f.p14.value)) 
   {   
	  alert("�`���ж�J�Ʀr");   
      return false;   
   }   
   
   
   if(f.p1.value =="")
   {	strMsg+="�u�k�ɽs���v"
      intFlag+=1}

	if(f.p2.value =="")
   {	strMsg+="�u����W�١v"
      intFlag+=1}
	
	if(f.p13.value =="")
   {	strMsg+="�u�C�W����v"
      intFlag+=1}
   
	if(f.p14.value =="")
   {	strMsg+="�u�`���v"
      intFlag+=1}
	
  
   if(intFlag!=0)
    {  alert("�ж�g"+strMsg+"���A���¡I")
		return false}

}


