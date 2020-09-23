import * as axios from "axios";
import Uri from "jsuri";
require("easy-autocomplete");

export default class people {
    init() {
        if ($('#people').length === 0) {
            return;
        }

        let instance = this;
        if ($('#people').html().trim()) {
            $("#mainContentArea").hide();
        } else {
            $("#mainContentArea").show();
        }

        let options = {
            data: names,
            list: {
                match: {
                    enabled: true
                },
                onChooseEvent: function () {
                    let keyword = $("#txtName").getSelectedItemData();
                    $("#txtName").val(keyword);
                    $("#btnSearch").click();
                }
            }
        };

        $("#txtName").easyAutocomplete(options);

        $("#btnSearch").click(function () {
            instance.doAjaxCallback();
        })

        $("#slSector").change(function () {
            $("#ulSector li").removeClass("active");
            $("[data-sector='" + $("#slSector").val() + "']").addClass("active");
        })

        $("#slLocation").change(function () {
            $("#ulLocation li").removeClass("active");
            $("[data-location='" + $("#slLocation").val() + "']").addClass("active");
        })

        $("#ulSector li").each(function () {
            $(this).click(function () {
                $("#ulSector li").removeClass("active");
                $(this).addClass("active");
                $("#slSector").val($(this).data("sector"));
                $("#btnSearch").click();
            })
        })

        $("#ulLocation li").each(function () {
            $(this).click(function () {
                $("#ulLocation li").removeClass("active");
                $(this).addClass("active");
                $("#slLocation").val($(this).data("location"));
                $("#btnSearch").click();
            })
        })
    }

    getFilterUrl() {
        let uri = new Uri(location.pathname);
        uri.replaceQueryParam("name", $("#txtName").val());
        uri.replaceQueryParam("sector", $("#slSector").val());
        uri.replaceQueryParam("location", $("#slLocation").val());
        return uri;
    }

    doAjaxCallback() {
        let instance = this;
        $('.loading-box').show();
        axios.get(instance.getFilterUrl())
            .then(function (result) {
                let fetched = $(result.data);
                $('#people').html(fetched.find('#people').html());
                if ($('#people').html().trim()) {
                    $("#mainContentArea").hide();
                }
                else {
                    $("#mainContentArea").show();
                }
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }
}

$('#btnClear').on("click",function() {
    $('#txtName').val('');
});