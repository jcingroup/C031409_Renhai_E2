<html>
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link rel="stylesheet" href="../../_Css/Set.css">
    <style type="text/css">
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

        .TableHeadTR {
            background-color: #1F6CBD;
            color: #FFFFFF;
            text-align: center;
            font-size: 12pt;
            padding-top: 3px;
            padding-bottom: 3px;
        }

        .TableHeadTD {
            border-style: solid;
            border-width: 1px;
            border-bottom-color: #333333;
            border-right-color: #CCCCCC;
        }

        .TableBodyTR {
            background-color: #EEF5FC;
            padding-top: 1px;
            padding-bottom: 1px;
            padding-left: 5px;
            padding-right: 5px;
        }

        .TableBodyTD {
            border-style: solid;
            border-width: 1px;
            border-bottom-color: #AAAAAA;
            border-right-color: #EEEEEE;
        }

        .TableBodyTdNum {
            text-align: right;
            border-style: solid;
            border-width: 1px;
            border-bottom-color: #AAAAAA;
            border-right-color: #EEEEEE;
        }
    </style>
</head>
<body>
    <!--#include file="../../_include/top.asp"-->
    <table class="gridtable" style="width: 99%; border: 1px">
        <tr>
            <td class="gridcaption" style="margin: 0px">
                <table style="width: 100%">
                    <tr>
                        <td class="gridtdcaption" id="CaptionText">禮斗訂購明細表
                        </td>
                        <td style="text-align: right"></td>
                         <td style="width: 80px; text-align: center">
                           <select id="sel_ProdSn">
                               <option value="">全部</option>
                               <option value="380">主斗</option>
                               <option value="381">斗首</option>
                               <option value="382">大斗</option>
                               <option value="383">中斗</option>
                               <option value="384">小斗</option>
                               <option value="385">福斗</option>
                           </select>
                        </td>
                        <td style="width: 80px; text-align: center">
                            <input type="button" id="btn_ExportExcel" class="button" value="禮斗名冊" style="margin-right: 10px;" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                <table style="width: 100%; margin: 1px; padding: 1px; border: 0px">
                    <!--Order Detail Start-->
                    <tr>
                        <td colspan="4">
                            <table id="TableProductDetailView" style="width: 100%; margin: 1px; padding: 1px; border: 1px">
                                <thead>
                                    <tr class="TableHeadTR">
                                        <td class="TableHeadTD">禮斗名稱</td>
                                        <td class="TableHeadTD">燈位名稱</td>
                                        <td class="TableHeadTD">姓名</td>
                                        <td class="TableHeadTD">住址</td>
                                        <td class="TableHeadTD">電話</td>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </td>
                    </tr>
                    <!--Order Detail End-->
                </table>
            </td>
        </tr>
    </table>
      <iframe id="ifm_exceldownload" src="" style="border: 0px; width: 0px; height: 0px"></iframe>
    <!--#include file="../../_include/bottom.asp"-->
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        $.ReFreshDetailHTML();
        $('#btn_ExportExcel').click(function () {
            var today = new Date();
            var parms = [];
            parms.push('year=' + today.getFullYear());
            parms.push('product_sn=' + $("select#sel_ProdSn option:checked").val());
            $("#ifm_exceldownload").attr("src", "../../../ExcelReport/LiDoRoll?" + parms.join('&'));
        });
    })
</script>
<script type="text/javascript">
    (function ($) {

        //取得統計資料
        $.ReFreshDetailHTML = function () {
            ajaxRequest.push
            (
		        $.ajax
		        (
			        {
			            url: '../../../Logic/ajax_getLiDo',
			            type: "POST",
			            datatype: "json",
			            contentType: "application/x-www-form-urlencoded; charset=utf-8",
			            data: {},
			            cache: false,
			            global: true,
			            error: function (jqXHR, textStatus, errorThrown) {
			                alert(errorThrown);
			            },
			            success: function (response) {
			                var stai = jQuery.parseJSON(response);
			                if (stai.result) {
			                    $('tbody', $('#TableProductDetailView')).html(''); //first clear
			                    for (var i = 0; i < stai.Module.length; i++) {

			                        $('tbody', $('#TableProductDetailView'))
                                    .append
                                    (
                                            $('<tr class="TableBodyTR">')
											    .append($('<td class="TableBodyTD">').html(stai.Module[i].產品名稱))
                                                .append($('<td class="TableBodyTD">').html(stai.Module[i].燈位名稱))
                                                .append($('<td class="TableBodyTdNum">').html(stai.Module[i].姓名))
                                                .append($('<td class="TableBodyTdNum">').html(stai.Module[i].地址))
                                                .append($('<td class="TableBodyTdNum">').html(stai.Module[i].電話))
			                        )
			                    }
			                } else {

			                    alert(stai.message);
			                }
			            }
			        }
		        )
	        )
        };
    })(jQuery);
</script>
