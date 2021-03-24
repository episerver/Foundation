class BulkUpdate {
    constructor() {
        this.contents = [];
    }

    init() {
        this.getContentTypes();
        this.getLanguages();
        $('.content-group-filter').on('change', { inst: this }, this.getContentTypes);
        $('.content-type-filter').on('change', this.getProperties);
        $('.button-apply-filters').on('click', { inst: this }, this.applyFilters);
    }
    
    getContentTypes(e) {
        var inst = typeof (e) !== 'undefined' ? e.data.inst : this;
        $('.loading-box').show();
        let filter = $('.content-group-filter').val();
        axios.get("bulkupdate/getcontenttypes/?type=" + filter)
            .then(function (result) {
                $('.content-type-filter').empty();
                $.each(result.data,
                    function (index, entry) {
                        if (entry.DisplayName != null) {
                            $('.content-type-filter')
                                .append($('<option>').attr('value', entry.ID).text(entry.DisplayName));
                        } else {
                            $('.content-type-filter').append($('<option>').attr('value', entry.ID).text(entry.Name));
                        }
                    });
                if (typeof $('.content-type-filter').val() !== 'undefined') {
                    inst.getDefaultProperties($('.content-type-filter').val());
                }
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

    getProperties(e) {
        axios.get("bulkupdate/getproperties/" + e.currentTarget.value)
            .then(function (result) {
                $('.table-content-info').empty();
                $('.table-content-info').append('<div style="text-align: center;"><h4>Click <b>"Apply Filter"</b> to get Bulk Update Contents</h4></div>');
                $('.checkbox-content-properties').empty();
                $.each(result.data, function (index, entry) {
                    var checkbox = `<label class="cb-container">`
                        + entry.Name +
                        `<input type="checkbox" value="` + entry.ID + `" name="` + entry.Name + `" />
                                     <span class="checkmark"></span>
                                     </label>`;
                    $('.checkbox-content-properties').append(checkbox);
                });
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
               
            });
    }

    getDefaultProperties(id) {
        axios.get("bulkupdate/getproperties/" + id)
            .then(function (result) {
                $('.checkbox-content-properties').empty();
                $.each(result.data, function (index, entry) {
                    var checkbox = `<label class="cb-container">`
                        + entry.Name +
                        `<input type="checkbox" value="` + entry.ID + `" name="` + entry.Name + `" />
                                    <span class="checkmark"></span>
                                    </label>`;
                    $('.checkbox-content-properties').append(checkbox);
                });
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {

            });
    }

    getLanguages() {
        $('.loading-box').show();
        axios.get("bulkupdate/getlanguages")
            .then(function (result) {
                $.each(result.data, function (index, entry) {
                    if (entry.DisplayName != null) {
                        $('.content-language-filter').append($('<option>').attr('value', entry.LanguageID).text(entry.DisplayName));
                    }
                    else {
                        $('.content-language-filter').append($('<option>').attr('value', entry.LanguageID).text(entry.Name));
                    }
                });
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

    applyFilters(e) {
        var inst = e.data.inst;
        $('.loading-box').show();
        var contentTypeId = $('.content-type-filter').val();
        var properties = [];
        $('.checkbox-content-properties input:checked').each(function () {
            properties.push($(this).attr("name"));
        });
        var language = $('.content-language-filter').val();
        var keyword = $('.content-name-filter').val();

        axios.get("bulkupdate/get?contentTypeId=" + contentTypeId + "&language=" + language + "&properties=" + properties.join() + "&keyword=" + keyword)
            .then(function (result) {
                inst.contents = result.data;
                if (result.data.length > 0) {

                    //Init content info
                    $.each(result.data, function (index, entry) {
                        inst.contents[index]["Properties"] = {};
                        $.each(properties, function (i, item) {
                            inst.contents[index]["Properties"][item] = entry[item].Value;
                        });
                    });

                    //Render header
                    var html = '<table class="table table-bordered table-content"><thead><tr><th>Id</th><th>Name</th>';
                    $.each(properties, function (i, entry) {
                        html += "<th>" + entry + "</th>";
                    });
                    html += "</tr></thead>";

                    html += "<tbody>";

                    //Render Edit All Row
                    html += '<tr><td></td>';
                    html += '<td>' + inst.generateInputTagForProperty(-1, "Name", "") + '</td>';
                    $.each(properties, function (i, item) {
                        if (result.data[0][item].PropertyDataType == "PropertyBoolean") {
                            html += '<td>' + inst.generateInputTagForProperty(-1, item, false) + '</td>';
                        } else {
                            html += '<td>' + inst.generateInputTagForProperty(-1, item, "") + '</td>';
                        }
                    });
                    html += '</tr>';

                    //Render content row
                    $.each(result.data, function (index, entry) {
                        html += '<tr><td>' + entry.ContentLink.Id + '</td>';
                        html += '<td>' + inst.generateInputTagForProperty(index, "Name", entry.Name) + '</td>';
                        $.each(properties, function (i, item) {
                            html += '<td>' + inst.generateInputTagForProperty(index, item, (typeof (entry[item]) == "object" && entry[item].PropertyDataType == "PropertyBoolean") ? (entry[item].Value == null ? false : entry[item].Value) : entry[item].Value) + '</td>';
                        });
                        html += '</tr>';
                    });
                    html += '</tbody></table>';

                    $('.table-content-info').empty();
                    $('.table-content-info').append("<input type='submit' class='btn btn-primary button-save-contents' value='Save all'>");
                    $('.table-content-info').append(html);
                    $('.table-content-info').append("<input type='submit' class='btn btn-primary button-save-contents' value='Save all'>");

                    //Set event
                    $('input[class*="prop"]')
                        .on('change', { inst: inst }, inst.updateContentInfo);
                    $('input[class*="button-save-content"]')
                        .on('click', { inst: inst }, inst.saveAll);

                    $('.grid-icon__loading').hide();
                } else {
                    $('.table-content-info').empty();
                    $('.table-content-info').append("<div style='text-align: center;'><h4><b>No content found</b></h4></div>");
                    $('.grid-icon__loading').hide();
                }
            })
            .catch(function (error) {
                notification.error(error);
                console.log(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

    generateInputTagForProperty(index, property, value) {
        let html = "";
        if (typeof (value) == "boolean") {
            if (value == true) {
                html = `<label class="cb-container">
                            <input class='prop' index="` + index + `" name='` + property + `' type='checkbox' checked />
                            <span class="checkmark"></span>
                            </label>`;
            }
            else {
                html = `<label class="cb-container">
                            <input class='prop' index="` + index + `" name='` + property + `' type='checkbox' />
                            <span class="checkmark"></span>
                            </label>`;
            }
        }
        else {
            if (value == null) {
                value = "";
            }
            html = "<input class='prop form-control' index=" + index + " name='" + property + "' type='text' value='" + value + "' />";
        }
        return html;
    }

    updateContentInfo(e) {
        var inst = e.data.inst;
        if (e.currentTarget.attributes["index"].value == -1) {
            if (e.currentTarget.type == "checkbox") {
                $('input[class*="prop"][name="' + e.currentTarget.attributes["name"].value + '"]').prop("checked", e.currentTarget.checked);
                $.each(inst.contents, function (index, item) {
                    item[e.currentTarget.attributes["name"].value].Value = e.currentTarget.checked;
                    item["Properties"][e.currentTarget.attributes["name"].value] = e.currentTarget.checked;
                });
            } else {
                $('input[class*="prop"][name="' + e.currentTarget.attributes["name"].value + '"]').val(e.currentTarget.value);
                $.each(inst.contents, function (index, item) {
                    if (e.currentTarget.attributes["name"].value != "Name") {
                        item[e.currentTarget.attributes["name"].value].Value = e.currentTarget.value;
                        item["Properties"][e.currentTarget.attributes["name"].value] = e.currentTarget.value;
                    } else {
                        item[e.currentTarget.attributes["name"].value] = e.currentTarget.value;
                    }
                });
            }
        }
        else {
            if (e.currentTarget.type == "checkbox") {
                inst.contents[e.currentTarget.attributes["index"].value][e.currentTarget.attributes["name"].value].Value = e.currentTarget.checked;
                inst.contents[e.currentTarget.attributes["index"].value]["Properties"][e.currentTarget.attributes["name"].value] = e.currentTarget.checked;
            } else {
                if (e.currentTarget.attributes["name"].value != "Name") {
                    inst.contents[e.currentTarget.attributes["index"].value][e.currentTarget.attributes["name"].value].Value = e.currentTarget.value;
                    inst.contents[e.currentTarget.attributes["index"].value]["Properties"][e.currentTarget.attributes["name"].value] = e.currentTarget.value;
                } else {
                    inst.contents[e.currentTarget.attributes["index"].value][e.currentTarget.attributes["name"].value] = e.currentTarget.value;
                }
            }
        }
    }

    saveAll(e) {
        var inst = e.data.inst;
        var c = confirm("Are you sure you want to update all contents?");
        if (c === true) {
            $('.loading-box').show();
            var properties = [];
            $('.checkbox-content-properties input:checked').each(function () {
                properties.push($(this).attr("name"));
            });

            var data = {
                Contents: inst.contents,
                Properties: properties.join()
            };
            axios.post("bulkupdate/updateContent", data)
                .then(function (result) {
                    $.each(result, function (index, entry) {
                        if (entry.DisplayName != null) {
                            $('.content-language-filter').append($('<option>').attr('value', entry.LanguageID).text(entry.DisplayName));
                        }
                        else {
                            $('.content-language-filter').append($('<option>').attr('value', entry.LanguageID).text(entry.Name));
                        }
                    });
                })
                .catch(function (error) {
                    notification.error(error);
                })
                .finally(function () {
                    $('.loading-box').hide();
                });
        }
    }
}; 