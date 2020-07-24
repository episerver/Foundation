import * as $ from "jquery";
import * as axios from "axios";

export default class Persons {
    constructor() {
        this.init();
    }

    init() {
        let instance = this;
        if ($('#persons').html().trim()) {
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
                $('#persons').html(fetched.find('#persons').html());
                if ($('#persons').html().trim()) {
                    $("#mainContentArea").hide();
                }
                else {
                    $("#mainContentArea").show();
                }
            })
            .catch(function (error) {
                notification.Error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }
}
