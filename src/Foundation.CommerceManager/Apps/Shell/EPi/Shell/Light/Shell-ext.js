
function reLoadCss() {
    $('div#DataForm table').css('background-color', 'transparent');
    $('div#mainDiv table').css('background-color', 'transparent');
    $('div#mainDiv td').css('background-color', 'transparent');

    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(0)').css('border', '1px solid #8b8b8b');
    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(1) > table > tbody > tr > td:eq(0)').css('border-top', '1px solid #8b8b8b');
    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(1) > table > tbody > tr > td:eq(0)').css('border-left', '1px solid #8b8b8b');
    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(1) > table > tbody > tr > td:eq(0)').css('border-right', '1px solid #8b8b8b');
    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(1) > table > tbody > tr > td:eq(0)').css('color', '#1D1D1D');
    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(1) > table > tbody > tr > td:eq(0)').css('font-family', '"Lucida Grande","Lucida Sans Unicode",Arial,Verdana,Sans-Serif');
    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(1) > table > tbody > tr > td:eq(1)').css('border-bottom', '1px solid #8b8b8b');

    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(1) > table > tbody > tr:eq(1) > td').css('border-left', '1px solid #8b8b8b');
    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(1) > table > tbody > tr:eq(1) > td').css('border-right', '1px solid #8b8b8b');
    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(1) > table > tbody > tr:eq(1) > td').css('border-bottom', '1px solid #8b8b8b');

    $('div#mainDiv span').css('color', '#1D1D1D');

    $('div#mainDiv > span > table:eq(2) > tbody > tr > td:eq(0) > span').each(function () {
        $(this).find('table > tbody > tr > td:eq(0) > div:eq(1)').css('background', 'url("../EPi/Shell/Resources/Gradients.png") repeat-x scroll left -5400px #F0F0F0');
    });
    $('div#mainDiv > span > table:eq(4) > tbody > tr > td:eq(0) > span > table > tbody > tr > td:eq(0) > div:eq(1)').css('background', 'url("../EPi/Shell/Resources/Gradients.png") repeat-x scroll left -5400px #F0F0F0');
    $('div#mainDiv > span > table:eq(4) > tbody > tr > td:eq(0) > div:eq(0) > table').css('border-color', '#8D8D8D');
    $('span#IbnMainLayout > div.LayoutBase > div > div > table > tbody > tr > td >  div#mainDiv > span > table:eq(0) > tbody > tr > td:eq(0) > div:eq(0)').css('background-color', '#FFFDBD');
    $('div#mainDiv > span > table:eq(0) > tbody > tr > td:eq(0) > div:eq(0)').css('border-bottom', '1px solid #8D8D8D');
    $('div#mainDiv > span > table:eq(0) > tbody > tr > td:eq(0) > div:eq(0)').css('color', '#000');

    /*=====================For NewOrder skin=============================*/
    $('span#IbnMainLayout > div > div > div > div > div > div > table:eq(1) > tbody > tr > td > table > tbody > tr > td > span > div').css('border', 'none');
    $('span#IbnMainLayout > div > div > div > div.popup-outer > div > div > table:eq(1) > tbody > tr > td > table > tbody > tr > td > span > div').css('margin-right', '2px');
    $('span#IbnMainLayout > div > div > div > div > div > div > table:eq(1) > tbody > tr > td > table > tbody > tr > td > span > div > div > table > tbody > tr > td').css('border-bottom', '1px solid #E6E6E6');
    $('span#IbnMainLayout > div > div > div > div > div > div > table:eq(1) > tbody > tr > td > table > tbody > tr > td > span > div > div > table > tbody > tr > td').css('height', '21px');
    $('span#IbnMainLayout > div.LayoutBase > div > div > div > div').css('border', 'none');
    $('span#IbnMainLayout > div.LayoutBase > div > span > div').css('border', 'none');
    $('span#IbnMainLayout > div.LayoutBase > div > div.report-view > div.report-content > div > div > div > table > tbody > tr > td > div > table:eq(0) > tbody > tr:eq(0) > td').css('zoom', '1');
    $('span#IbnMainLayout > div.LayoutBase > div > span:eq(0) > div.DockContainer > div > table.filter > tbody > tr:eq(0) > td > div > table.ibn-propertysheet > tbody > tr > td:eq(0) > input').css('border', '1px solid #B4B4B4');
    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(0) > div').each(function () {
        if ($(this).css('border') != undefined && $(this).css('border').indexOf('1px solid') >= 0) {
            $(this).css({ 'border': '1px solid #8B8B8B', 'background': 'url("../EPi/Shell/Resources/Gradients.png") repeat-x scroll left -2200px #FAA61D' });
        }
    });

    /*=====================For OrderDetail skin=============================*/
    $('div#mainDiv > span > table:eq(2) > tbody > tr > td > div table:eq(0)').css('border-color', '#8D8D8D');
        
    
    $('div#mainDiv > div > span > table:eq(1) > tbody > tr > td:eq(1) > table > tbody > tr:eq(1) > td > table > tbody > tr:eq(0) > td > table > tbody > tr > td > div > span > div').css('border', 'none');
    $('div.modalPopupWindow').css('padding-left', '1px');
    $('div.modalPopupWindow').css('background-color', '#E9E9E9');
    $('td.orderform-field > div').css('border', '1px solid #999999');
    $('td.orderform-field > div > table > tbody > tr > td').css('border-bottom', '1px solid #999999');
    $('td.orderform-field > table > tbody > tr > td:eq(1).btndown').css('background-color', '#F0F0F0');
    $('span#IbnMainLayout  table#addressContainerTable select').css('width', '230px');
    /*===================================================================*/
    $('div#contentPanel > table > tbody > tr > td > table > tbody > tr > td > table > tbody > tr > td > span > div').css('border', 'none');
    $('div#contentPanel > div.popup-outer > div > div > div > table > tbody > tr > td > table > tbody > tr > td > span > div').css('border', 'none');
    $('div#contentPanel > div > div.popup-outer > div > div > table > tbody > tr > td > table > tbody > tr > td > span > div').css('border', 'none');
    $('div#contentPanel > div.popup-outer > div > div > div > table > tbody > tr > td > table > tbody > tr > td > span > div > div:eq(0) > div:eq(0)').css('float', 'left');
}