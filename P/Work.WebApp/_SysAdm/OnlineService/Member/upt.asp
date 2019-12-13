<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<%
On Error Resume Next
Dim StrSql,oRs
Dim WhereObj,strwhere
Dim strQS,strScript,strPageNo,Uptmode
Dim strReturn,strRS
Dim aryIns,aryDel
Dim p0,p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12,p13,p14,p15,p16,p17,p18,p19,p20,p21,p22
Dim p14Option,p13Option,p17Option
Dim MasterID

	MasterID = Request.QueryString("p0")

	If MasterID = "" Then
		Uptmode = "Insert"	'新增模式
		MasterID = GetSysID("會員戶長資料")
	Else
		Uptmode = "Update"	'修改模式
	End If

'=========================

	p6 = "320"
	p13 = "桃園市"
	p14 = "中壢區"

	
	StrSql="SELECT 縣市,縣市 FROM 地址縣市 order by 排序 "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	p13Option = RsToOption(oRs,0,1,Cstr(p13),"","")
	AspErrCheck "p13Option"
		
	StrSql="SELECT 鄉鎮,鄉鎮 FROM 地址鄉鎮 Where 縣市='" & p13 & "' order by 排序 "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	p14Option = RsToOption(oRs,0,1,Cstr(p14),"","")
	AspErrCheck "p14Option"	
	

    StrSql="SELECT member_detail_category_id,member_detail_category_name FROM Member_Detail_Category WHERE member_detail_category_id!=4"
    Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
    p17Option = RsToOption(oRs,0,1,Cstr(p17),"","")
    AspErrCheck "p17Option"

	'-- 回上一頁
	strQS="MasterID=" & MasterID & "&pageno=" & strPageNo
	strReturn="<a href=""list.asp?"&strQS&"&pageno="&strPageNo&""">←回上一頁</a>"	
	

%>
<html>
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link rel="stylesheet" href="../../_CSS/default.css">
    <script type="text/javascript" src="lunar.js"></script>
    <style>
        .GridTable {
            margin-top: 10px;
            border: 5 double #778899;
        }

        TH {
            background: #F5F5DC;
        }


        .TDLine {
            background-color: #FF7573;
            color: #FFFFFF;
            text-align: center;
            font-size: 11pt;
        }
    </style>
    <!-- 這一段要放在HEAD裡面 -->
</head>
<body>
    <%'=strReturn %>
    <input type="button" value="回上一頁" onclick="history.back()">
    <font color="red">*使用說明：1.請先確定戶長及家庭成員人數姓名，「安太歲」農曆生日生辰要正確。2.確認後「新增訂單」。</font>
    <div style="color: red; font-size: 12pt">會員每個人的"地址欄位"不可空白，無法得知地址可隨意填入任意字元!!!</div>
    <!--#include file="../../_include/top.asp"-->
    <!-- Grid List區段 -->
    <form id="frmGrid" name="frmGrid" action="ifrm_del.member.asp" method="post">
        <input type="hidden" name="MemberCount" id="MemberCount" value="0">
        <input type="hidden" name="MasterID" id="MasterID" value="<%=MasterID%>">
        <div id="RsToTabel">
        </div>
    </form>
    <!-- Grid List區段 -->
    <form id="myform" name="myform" action="ajax.myform.submit.asp" method="post">
        <input type="hidden" id="Uptmode" name="Uptmode" value="">
        <input type="hidden" name="cmd" value="set">
        <input type="hidden" id="p0" name="p0" value="">
        <table id="EditDataTable" class="gridtable" width="99%" border="1" cellspacing="0"
            cellpadding="0">
            <caption class="gridcaption style:margin:0">
                <table width="100%">
                    <tr>
                        <td class="gridtdcaption" id="CaptionText"></td>
                        <td align="right"></td>
                    </tr>
                </table>
            </caption>
            <tr>
                <td valign="top">
                    <table width="100%" border="0" cellspacing="1" cellpadding="1">
                        <tr>
                            <th width="90"></th>
                            <td width="210"></td>
                            <th width="120"></th>
                            <td width="180"></td>
                        </tr>
                        <!--<tr><td colspan="4" class="TDLine">基本資料填寫</td></tr>-->
                        <tr>
                            <th align="right">戶長ID：
                            </th>
                            <td>
                                <%=MasterID%><input type="hidden" name="MasterID" value="<%=MasterID%>" />
                            </td>
                            <th align="right">是否為戶長：
                            </th>
                            <td>
                                <input type="checkbox" name="p1" id="p1" value="1" <%=p1Check%> />
                            </td>
                        </tr>
                        <tr>
                            <th align="right">姓名：
                            </th>
                            <td>
                                <input type="text" name="p2" id="p2" value="<%=p2%>" maxlength="20" size="20" />
                                <font color="red">*姓名最長20個字元，外籍人士請簡寫。</font>
                            </td>
                            <th align="right">性別：
                            </th>
                            <td>
                                <input type="radio" name="p3" id="p3A" value="1" <%=p3RadioA%> />先生
                            <input type="radio" name="p3" id="p3B" value="2" <%=p3RadioB%> />小姐
                            </td>
                        </tr>
                        <tr>
                            <th align="right">生日：
                            </th>
                            <td>
                                <input type="radio" name="p15" id="p15A" value="國" />國曆
                            <input type="radio" name="p15" id="p15B" value="農" />農曆
                            <input type="text" name="p4" id="p4" value="<%=p4%>" maxlength="10" size="8" />
                                <input type="checkbox" name="p19" id="p19" value="1" />閏月
                            <select id="p16" name="p16">
                                <option value="鼠">鼠</option>
                                <option value="牛">牛</option>
                                <option value="虎">虎</option>
                                <option value="兔">兔</option>
                                <option value="龍">龍</option>
                                <option value="蛇">蛇</option>
                                <option value="馬">馬</option>
                                <option value="羊">羊</option>
                                <option value="猴">猴</option>
                                <option value="雞">雞</option>
                                <option value="狗">狗</option>
                                <option value="豬">豬</option>
                            </select>
                                <input type="button" name="btn_NoBirthday" id="btn_NoBirthday" value="不填生日" />
                                <br />
                                <font color="red">*安太歲、入斗保運一定要打生日，請輸入民國年(格式:66/1/1)</font>
                            </td>
                            <th align="right">生辰：
                            </th>
                            <td>
                                <select name="p5" id="p5">
                                    <option value="吉">00:00~23:59 吉時</option>
                                    <option value="子">23:00~01:00 子時</option>
                                    <option value="丑">01:00~03:00 丑時</option>
                                    <option value="寅">03:00~05:00 寅時</option>
                                    <option value="卯">05:00~07:00 卯時</option>
                                    <option value="辰">07:00~09:00 辰時</option>
                                    <option value="巳">09:00~11:00 巳時</option>
                                    <option value="午">11:00~13:00 午時</option>
                                    <option value="未">13:00~15:00 未時</option>
                                    <option value="申">15:00~17:00 申時</option>
                                    <option value="酉">17:00~19:00 酉時</option>
                                    <option value="戌">19:00~21:00 戌時</option>
                                    <option value="亥">21:00~23:00 亥時</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <th align="right">地址：
                            </th>
                            <td colspan="3">
                                <input type="text" name="p6" id="p6" size="5" value="<%=p6%>" />-
                            <select id="p13" name="p13">
                                <%=p13Option%>
                            </select>
                                <select id="p14" name="p14">
                                    <%=p14Option%>
                                </select>
                                <input type="text" name="p7" id="p7" value="<%=p7%>" maxlength="96" size="32" />
                                <button id="btn_celarAddress" type="button">清除地址</button>
                                <button id="btn_getMasterAddress" type="button">同戶長</button>
                                <script type="text/javascript">

                                    $(document).ready(function () {

                                        $('#btn_celarAddress').click(function () {
                                            $('#p7').val('');
                                            $('#p13').val('').trigger('change');
                                            $('#p14').val('');
                                            $('#p6').val('');
                                        });

                                        $('#btn_getMasterAddress').click(function () {

                                            var jqxhr = $.ajax({
                                                type: "GET",
                                                url: '../../../Member/GetMemberByDetail?member_id=' + $('#MasterID').val(),
                                                data: {},
                                                dataType: 'json'
                                            })
                                            .done(function (data, textStatus, jqXHRdata) {
                                                console.log(data);
                                                $('#p7').val(data.data.address);
                                                $('#p13').val(data.data.city).trigger('change');
                                                $('#p14').val(data.data.country);
                                                $('#p6').val(data.data.zip);
                                            });
                                        });

                                    });
                                </script>
                                <br>
                                <span style="color: red">*戶長一定要打地址</span>
                            </td>
                        </tr>
                        <tr>
                            <th align="right">手機：
                            </th>
                            <td>
                                <input type="text" name="p8" id="p8" value="<%=p8%>" maxlength="16" size="16" />
                            </td>
                            <th align="right">電話：
                            </th>
                            <td>
                                <input type="hidden" name="p11" id="p11" value="" />
                                <input type="text" name="p9" id="p9" value="<%=p9%>" maxlength="16" size="16" />
                            </td>
                        </tr>
                        <tr>
                            <th align="right">EMAIL：
                            </th>
                            <td colspan="3">
                                <input type="text" name="p10" id="p10" value="<%=p10%>" maxlength="64" size="64" />
                            </td>
                        </tr>
                        <tr>
                            <th align="right">身分別：
                            </th>
                            <td colspan="3">
                                <select id="p17" name="p17">
                                    <%=p17Option%>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <th align="right">身分證字號：
                            </th>
                            <td>
                                <input type="text" name="p21" id="p21" value="<%=p21%>" maxlength="10" size="16" />
                            </td>
                            <th align="right">國曆投保生日：
                            </th>
                            <td>
                                <input type="text" name="p22" id="p22" value="<%=p22%>" maxlength="20" size="16" />
                                 <br />
                                <font color="red">*請輸入民國年(格式:66/1/1)</font>
                            </td>
                        </tr>

                        <tr>
                            <th align="right">備註：
                            </th>
                            <td colspan="3">
                                <textarea id="p20" name="p20" cols="70"></textarea>
                            </td>
                        </tr>


                        <tr>
                            <td class="gridtdtool" colspan="4" align="Center">
                                <span class="errmsg">
                                    <%=strMsg%></span><br />
                                <input id="btn_submit" name="btn_submit" class="gridsubmit" type="button" value="     確      認     "
                                    style="cursor: Hand" />
                                <input type="button" id="CancelEdit" value="取消編輯" class="gridsubmit" style="cursor: Hand" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
<script type="text/javascript">

    function RedireOrder() {
        document.location.href = '../../../Orders#/edit/gorders?member_id=' + $('#MasterID').val() + "&t=" + (new Date()).getTime();
    }
    function RedireOrder_DouLight() {
        document.location.href = '../../../Orders#/edit/doulightorders?member_id=' + $('#MasterID').val() + "&t=" + (new Date()).getTime();
    }

    function RedireOrder_Fortune() {
        document.location.href = '../../../Orders#/edit/forders?member_id=' + $('#MasterID').val() + "&t=" + (new Date()).getTime();
    }

    function RedireOrderM() {
        //document.location.href = '../Order_MD/Upt.asp?MasterID=' + $('#MasterID').val() + "&t=" + (new Date()).getTime();
        document.location.href = '../../../Orders#/edit/mdorders?member_id=' + $('#MasterID').val() + "&t=" + (new Date()).getTime();
    }

    function RedireOrderMBMS() {
        //document.location.href = '../Order_MBMS/Upt.asp?MasterID=' + $('#MasterID').val() + "&t=" + (new Date()).getTime();
        document.location.href = '../../../Orders#/edit/sdorders?member_id=' + $('#MasterID').val() + "&t=" + (new Date()).getTime();
    }
    function RedireOrderWishLight() {
        document.location.href = '../../../Orders#/edit/wishorders?member_id=' + $('#MasterID').val() + "&t=" + (new Date()).getTime();
    }
    //按下新增成員
    function AddNewMember() {
        $('#Uptmode').val('Insert');
        $('#p0').val('');

        $('#p2').val('');
        $('#p13').val('桃園市');
        //countryName = '中壢市';
        $('#p13').trigger('change');



        $('#p7').val('');
        $('#p1').attr('checked', false);
        $('#p4').val('');
        $('#p5').val('吉');
        $('#p8').val('');
        $('#p9').val('');
        $('#p10').val('');

        $('#p3A').attr('checked', false);
        $('#p3B').attr('checked', false);
        $('#p15B').attr('checked', true);
        $('#p20').val('');

        var data_jsonstyle = { MasterID: $('#MasterID').val() };
        //此Ajax主要為新增成員時 可先放入預設的資料
        $.EventAjaxHandle(data_jsonstyle, "ajax.get.master.data.asp")
        .done(function (data, textStatus, jqXHR) {

            var jsonobj = jQuery.parseJSON(data);
            if (jsonobj.result == true) {

                AllowEdit();
                $('#btnins2').attr('disabled', true); //新增Button Disabled 
                $('#btnins0').attr('disabled', true); //新增Button Disabled 
                if (jsonobj.IsData) {

                    $('#p13').val(jsonobj.縣市);
                    $('#p13').trigger('change');
                    $('#p14').val(jsonobj.鄉鎮);
                    countryName = jsonobj.鄉鎮;
                    $('#p7').val(jsonobj.地址);
                    $('#p9').val(jsonobj.電話尾碼);
                    $('#CaptionText').text("會員管理：新增");

                } else {
                    $('#p1').attr('checked', true);
                }
                $('#p2').focus(); //新增家庭成員 姓名會成 focus
            }
            else { alert(jsonobj.message); }
        })

        var SupportDiv = document.getElementById('EditDataTable');
        window.scroll(0, findPos(SupportDiv));
    }


    $(document).ready(function () {
        var countryName = '';

        $('#btn_SearchData').click(function () {
            AjaxSearchData();
        });
        EnterDownData();
    })

    function EnterDownData() {
        var data_jsonstyle = { MasterID: $('#MasterID').val() };
        $.EventAjaxHandle(data_jsonstyle, "ifrm_list.ajax.load.data.asp?t=" + (new Date()).getTime())
        .done(function (data, textStatus, jqXHR) {
            var jsonobj = jQuery.parseJSON(data);
            if (jsonobj.result == true) {
                $('#RsToTabel').html(jsonobj.GridList);
                $('#MemberCount').val(jsonobj.RecordCount);

                //將西元 - 1911
                $('.LBirthday').each(function (index) {
                    var LBirthdayArray = $(this).text().split('/');
                    var LBirthdayYear = parseInt(LBirthdayArray[0], 10) - 1911;

                    $(this).text(LBirthdayYear + '/' + LBirthdayArray[1] + '/' + LBirthdayArray[2]);
                })

                if (jsonobj.RecordCount == 0) { //如果都沒有家庭成員 則新增訂單鍵取消
                    $('#btnins0').attr('disabled', true);
                } else {
                    $('#btnins0').attr('disabled', false);
                }
            }
            else { alert(jsonobj.message); }
        })

    }

    //這個是給 換頁Select Box Ajax使用
    $('#PageNo').live('change', function () {
        AjaxChangePage($('#PageNo').val());
    })

    function AjaxChangePage(page) {

        var data_jsonstyle = $.fn.CollectQuery();
        data_jsonstyle['pageno'] = page;
        data_jsonstyle['MasterID'] = $('#MasterID').val();

        $.EventAjaxHandle(data_jsonstyle, "ifrm_list.ajax.load.data.asp?t=" + (new Date()).getTime())
        .done(function (data, textStatus, jqXHR) {
            var jsonobj = jQuery.parseJSON(data);
            if (jsonobj.result == true) {
                $('#RsToTabel').html(jsonobj.GridList);

                //將西元 - 1911
                $('.LBirthday').each(function (index) {
                    var LBirthdayArray = $(this).text().split('/');
                    var LBirthdayYear = parseInt(LBirthdayArray[0], 10) - 1911;
                    $(this).text(LBirthdayYear + '/' + LBirthdayArray[1] + '/' + LBirthdayArray[2]);
                })
            }
            else {
                alert(jsonobj.message);
            }
        })
    }

    function AjaxSearchData() {
        var data_jsonstyle = $.fn.CollectQuery();
        $.EventAjaxHandle(data_jsonstyle, "ifrm_list.ajax.load.data.asp")
        .done(function (data, textStatus, jqXHR) {
            var jsonobj = jQuery.parseJSON(data);

            if (jsonobj.result == true) {
                $('#RsToTabel').html(jsonobj.GridList);
            }
            else {
                alert(jsonobj.message);
            }
        })
    }

    //修改會員資料 Ajax程序
    function AjaxGetMemberData(id) {

        if ($('#Uptmode').val() == 'Insert') {
            alert('新增模式下無法修改');
        }
        else {
            var data_jsonstyle = { MemberID: id };
            $.EventAjaxHandle(data_jsonstyle, "ajax.get.member.data.asp")
            .done(function (data, textStatus, jqXHR) {
                var jsonobj = jQuery.parseJSON(data);

                if (jsonobj.result == true) {
                    $('#btnins2').attr('disabled', true); //修改模式，則新增禁止
                    $('#btnins0').attr('disabled', true); //修改模式，則新增禁止

                    AllowEdit();
                    $('#p0').val(jsonobj.序號);
                    $('#p2').val(jsonobj.姓名);

                    $('#p13').val(jsonobj.縣市);
                    countryName = jsonobj.鄉鎮;
                    $('#p13').trigger('change');

                    $('#p7').val(jsonobj.地址);
                    $('#p1').attr('checked', jsonobj.Is戶長);
                    $('#p19').attr('checked', jsonobj.isOnLeapMonth);
                    $('#p4').val(jsonobj.生日);
                    $('#p5').val(jsonobj.時辰);
                    $('#p8').val(jsonobj.手機);
                    $('#p9').val(jsonobj.電話尾碼);
                    $('#p10').val(jsonobj.EMAIL);
                    $('#p16').val(jsonobj.生肖);
                    $('#p6').val(jsonobj.郵遞區號);
                    $('#p20').val(jsonobj.祈福事項);
                    $('#p21').val(jsonobj.sno);
                    $('#p22').val(jsonobj.insure_birthday);

                    if (jsonobj.性別 == '1') $('#p3A').attr('checked', true);
                    if (jsonobj.性別 == '2') $('#p3B').attr('checked', true);

                    $('#p15B').attr('checked', true);

                    $('#p17').val(jsonobj.member_detail_category_id);

                    $('#Uptmode').val('Update');
                    $('#CaptionText').text("家庭成員管理：修改(修改後需點「確認」或「取消編輯」)");
                }
                else { alert(jsonobj.message); }
            })

            var SupportDiv = document.getElementById('EditDataTable');
            window.scroll(0, findPos(SupportDiv));
        }
    }
</script>
<script type="text/javascript">
    $(document).ready(function () {
        //縣市 鄉鎮 地址切換
        $(function () {
            $('#p13').change(function () {

                $('#p14').removeOption(/.?/).ajaxAddOption(
        	        	'ajax_GetAddressCountry.asp',
        	        	{ 'id': $(this).val() },
        	        	false,
        	        	function () {
        	        	    // 設定預設選項
        	        	    $(this).selectOptions(countryName).trigger('change');
        	        	}
        	    	)
            })
        })

        //取得郵遞區號
        $('#p14').change(function () {
            var data_jsonstyle = { id: $('#p13').val(), ids: $('#p14').val() };
            $.EventAjaxHandle(data_jsonstyle, "ajax_GetAddressZip.asp")
            .done(function (data, textStatus, jqXHR) {
                var jsonobj = jQuery.parseJSON(data);
                if (jsonobj.result == true) {
                    $('#p6').val(jsonobj.Zip);
                }

                if (jsonobj.message != '') {
                    alert(jsonobj.message);
                }
            })
        })

        $("#myform").validate({
            rules:
			{
			    p2: { required: true },
			    p10: { email: true }
			},
            messages:
			{
			}
        });

        $('#btn_submit').click(function () {
            //待所有的ajax完成 才可做儲檔的動作
            ajaxHasDone = $.when.apply($, ajaxRequest);

            ajaxHasDone.done(function () {
                $.fn.activeMyForm();
            });
        });

        (function ($) {
            $.fn.activeMyForm = function () {
                if ($('#myform').valid()) {

                    if ($('#p1').prop('checked') && ($('#p7').val().trim() == '' || $('#p6').val().trim() == '')) {
                        FormFlag = false;
                        alert('戶長需填寫正確地址及郵遞區號!');
                        return;
                    }

                    if ($('#p3A').prop('checked') == false && $('#p3B').prop('checked') == false) {
                        alert('請選擇性別!');
                        return;
                    }

                    if (!VialDate() && FormFlag) {
                        alert('生日日期格式輸入不正確!!!');
                        return;
                    }

                    if (!checkTwID($('#p21').val())) {
                        alert('身分證字號格式輸入不正確!!!');
                        return;
                    }

                    if (!checkDate($('#p22').val())) {
                        alert('國立投保生日格式輸入不正確!!!');
                        return;
                    }

                    var gDate = $('#p4').val().split('/');

                    if ($('input[name$="p15"]:checked').val() == '國') {
                        Lunar(0, parseInt(gDate[0], 10) + 1911, gDate[1], gDate[2]);
                        $('#p4').val((lunar.y - 1911) + '/' + lunar.m + '/' + lunar.d);
                    } else {
                        //$('#p4').val( (parseInt(gDate[0],10)+1911) + '/' + gDate[1] + '/' + gDate[2]);
                    }
                    $('#myform').submit();
                }
            }
        })(jQuery);

        $('#myform').submit(function () {
            $(this).ajaxSubmit(options);
            return false;
        });

        var options = {
            target: '#outputmessagefromserver',
            beforeSubmit: showRequest,
            success: showResponse,
            dataType: 'json'
        };

        function showRequest(formData, jqForm, options) {
            var queryString = $.param(formData);
            return true;
        }

        //表單在Server完成 傳送Data回來，由此function處理
        function showResponse(jsonobj, statusText, xhr, $form) {
            //
            if (jsonobj.result) {
                $('#p0').val(jsonobj.InsertID);
                $('#Uptmode').val("Update");
                $('#p15B').attr('checked', true);
                EnterDownData();
                StatusInit();
                alert('更新完成');
                NotEdit(); //更新完成後 就不可編輯
                //alert(jsonobj.InsertID);
            } else {
                alert(jsonobj.message);
            }
        }
        //ajax主要表單結束


        //Grid表單填寫規則設定
        //$("#frmGrid").validate({
        //	rules:	{},
        //	messages:{}
        //});

        var frmGrid_options = {
            beforeSubmit: showRequest_frmGrid,
            success: showResponse_frmGrid,
            dataType: 'json'
        };

        $('#cmd').live('click', function () {
            if (confirm("確定要刪除資料？")) {
                if ($('#frmGrid').valid()) {
                    $('#frmGrid').submit();
                }
            }
        });

        $('#frmGrid').submit(function () {
            $(this).ajaxSubmit(frmGrid_options);
            return false;
        });

        function showRequest_frmGrid(formData, jqForm, frmGrid_options) {
            var queryString = $.param(formData);
            return true;
        }

        //表單在Server完成 傳送Data回來，由此function處理
        function showResponse_frmGrid(jsonobj, statusText, xhr, $form) {
            //
            if (jsonobj.result) {
                EnterDownData();
                alert('刪除完成');
                //alert(jsonobj.InsertID);
            } else {
                alert(jsonobj.message);
            }
        }
        //ajax主要表單結束

        //取消編輯
        $('#CancelEdit').click(function () {

            $('#Uptmode').val('');
            $('#btnins2').attr('disabled', false); //開起新增 
            if ($('#MemberCount').val() != '0') {
                $('#btnins0').attr('disabled', false); //開起新增
            }
            NotEdit();
        })


        //無生日
        $('#btn_NoBirthday').click(function () {

            $('#p4').val('1/1/1');
            $('#p14B').attr('checked', true);
            $('#p16').val('鼠');
        })

        //填完生日後取得生肖
        $('#p4').blur(function () {
            if (VialDate()) {
                var gDate = $('#p4').val().split('/');
                if ($('input[name$="p15"]:checked').val() == '國') {
                    Lunar(0, parseInt(gDate[0], 10) + 1911, gDate[1], gDate[2]);
                    var GetLurDateYear = (lunar.y - 1911);
                } else {
                    var GetLurDateYear = gDate[0];
                }

                var data_jsonstyle = { LurYear: GetLurDateYear };
                $.EventAjaxHandle(data_jsonstyle, "ajax_GetYearAnimal.asp")
                .done(function (data, textStatus, jqXHR) {
                    var jsonobj = jQuery.parseJSON(data);

                    if (jsonobj.result == true) {
                        if (jsonobj.IsData) {
                            $('#p16').val(jsonobj.生肖);
                        }
                    }

                    if (jsonobj.message != '') {
                        alert(jsonobj.message);
                    }
                })
            }
        })

        //狀態設定
        function StatusInit() {

            if ($('#Uptmode').val() == "Insert") {
                $('#btn_submit').val("確認");
                $('#CaptionText').text("會員管理：新增");
            }

            if ($('#Uptmode').val() == "Update") {
                $('#btn_submit').val("確認");
                $('#CaptionText').text("會員管理：修改");
            }
        }

        StatusInit();
        NotEdit(); //一開始進來是禁止編輯
    });

    function AllowEdit() {
        $('#myform').attr('disabled', false);
        $('#myform input').attr('disabled', false);
        $('#myform select').attr('disabled', false);
        $('#myform textarea').attr('disabled', false);
    }

    function NotEdit() {
        $('#myform').attr('disabled', true);
        $('#myform input').attr('disabled', true);
        $('#myform select').attr('disabled', true);
        $('#myform textarea').attr('disabled', true);
    }

    function trim(txt) {
        var left, right;
        var txt2 = ""
        for (i = 0; i < txt.length; i++) {
            if (txt.charAt(i) == " ") {
                continue;
            } else {
                left = i;
                break;
            }
        }
        for (i = txt.length - 1; i >= 0; i--) {
            if (txt.charAt(i) == " ") {
                continue;
            } else {
                right = i;
                break;
            }
        }
        for (i = left; i <= right; i++) {
            txt2 = txt2 + txt.charAt(i);
        }
        return txt2;
    }


    function VialDate() {

        var gDate = $('#p4').val().split('/');

        if (gDate.length != 3) {
            return false;
        }
        else {
            if (isNaN(gDate[0]) || isNaN(gDate[1]) || isNaN(gDate[2]) || trim(gDate[0]) == '' || trim(gDate[1]) == '' || trim(gDate[2]) == '') {
                return false;
            }
            else {
                return true;
            }
        }
    }
    function checkTwID(id) {
        if (id != null && id != "") {
            //*****************
            // 台灣身份證檢查簡
            //*****************

            //建立字母分數陣列(A~Z)
            var city = new Array(
                1, 10, 19, 28, 37, 46, 55, 64, 39, 73, 82, 2, 11,
                20, 48, 29, 38, 47, 56, 65, 74, 83, 21, 3, 12, 30
                )
            id = id.toUpperCase();
            // 使用「正規表達式」檢驗格式
            if (id.search(/^[A-Z](1|2)\d{8}$/i) == -1) {
                //alert('基本格式錯誤');
                return false;
            } else {
                //將字串分割為陣列(IE必需這麼做才不會出錯)
                id = id.split('');
                //計算總分
                var total = city[id[0].charCodeAt(0) - 65];
                for (var i = 1; i <= 8; i++) {
                    total += eval(id[i]) * (9 - i);
                }
                //補上檢查碼(最後一碼)
                total += eval(id[9]);
                //檢查比對碼(餘數應為0);
                return ((total % 10 == 0));
            }
        } else {
            return true;
        }
    }

    function checkDate(date) {//檢查日期
        if (date != null && date != "") {
            var day = date.split('/');
            var month = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

            if (checkLeapYear(parseInt(day[0]) + 1911)) {//如果閏年2月變29天
                month[1] = 29;
            }

            if (day[1] > 12 || day[1] <= 0) {//
                return false;
            } else if (day[2] > month[(day[1] - 1)] || day[2] <= 0) {
                return false;
            } else {
                return true
            }
        } else {
            return true;
        }

    }
    function checkLeapYear(Year) {//檢查閏年true:是閏年
        if (Year % 4 == 0) {
            if (Year % 100 == 0) {
                if (Year % 400 == 0) {
                    return true;//可以被100整除又可以被400整除
                } else {
                    return false;//可以被100整除但無法被400整除
                }
            } else {
                return true;//被4整除但無法被100整除
            }
        } else {//無法被4整除
            return false;
        }
    }

</script>
<script type="text/javascript" language="javascript">
    //顯示讀取資料中..... Show and Hide
    $("#wait").center();

    $("#wait").ajaxStart(function () {
        $("#wait").css("display", "block");
    });

    $("#wait").ajaxComplete(function () {
        $("#wait").css("display", "none");
    });
    //讀取資料 Show and Hide
    $("#wait").center();

    $("#wait").ajaxStart(function () {
        $("#wait").css("display", "block");
    });

    $("#wait").ajaxComplete(function () {
        $("#wait").css("display", "none");
    });
</script>
