var userFilterType;
var userFilterValue;
var isManager = null;

function selectCategorie() {
    $('.categories').children('div.selected').removeClass('selected');
    $(this).addClass('selected');
    var selectedCat = $(this).attr('id');
    $('#sortable1').children('li').each(function () {
        if ($(this).attr('categorie') === selectedCat) {
            $(this).show();
        } else { $(this).hide(); }
    });
    if ($('ul#sortable2').height() < $('ul#sortable1').height()) {
        $('ul#sortable2').height($('ul#sortable1').height());
    }
    else {
        $('ul#sortable2').removeAttr('style');
    }
}

function setAvailableApplications(response) {
    $('#sortable1').delay(700).empty();
    $('.categories').empty();
    var selectedCat;
    if (response.length === 0) {
        $('div.available-apps').children('div').addClass('no-result');
        $('<div><span>Veuillez changer le filtre pour séléctionner des applications.</span></div>').appendTo('#sortable1');
    }
    else {
        if ($('div.available-apps').children('div').hasClass('no-result')) {
            $('div.available-apps').children('div').removeClass('no-result');
        }
        for (var i = 0; i < response.length; i++) {
            var categoryId = response[i].Categorie.replace(/\s/g, '');
            var categoryFound = false;
            $('.categories').children('div').each(function () {
                if ($(this).attr('id') === categoryId) {
                    categoryFound = true;
                }
            });
            if (categoryFound === false) {
                var categorie = $('<div id=' + categoryId + '><span>' + response[i].Categorie + '</span></div>').bind('click', selectCategorie);
                categorie.appendTo('.categories');
                if (i === 0) {
                    selectedCat = categoryId;
                    categorie.addClass('selected');
                }
            }
            var newItem = $('<li id="' + response[i].Id + '" class="grab" categorie="' + categoryId + '"><div>' + response[i].Name + '</div></li>');
            if (response[i].Description !== null && response[i].Description !== "") {
                newItem.attr('description', response[i].Description);
                newItem.bind('mouseover', function () {
                    var desc = $(this).attr('description');
                    $(this).append("<div class='info' style='position:absolute;top:0'>" + response[i].Name + "</div>");
                });
                newItem.bind('mouseout', function () {
                    $(this).children('div.info').remove();
                });
            }
            if (categoryId !== selectedCat) {
                newItem.appendTo('#sortable1').hide();
            }
            else {
                newItem.hide().appendTo('#sortable1').slideDown("slow");
            }
        }
    }
}

function getAvailableApplications() {
    var defaultDataParameters = { search: 'application' };
    var allObjects = [{ Name: "Link 1", Categorie: "category1", Id: "1", Description: "Description of link 1" },
                      { Name: "Link 2", Categorie: "category1", Id: "2", Description: "Description of link 2" },
                      { Name: "Link 3", Categorie: "category2", Id: "3", Description: "Description of link 3" },
                      { Name: "Link 4", Categorie: "category2", Id: "4", Description: "Description of link 4" }];
    setAvailableApplications(allObjects);
}

$(function () {
    $('ul').each(function () {
        if ($(this).children('li').length === 1) {
            $(this).children('li.emptyMessage').show();
        }
    });
    $("#sortable1").sortable({
        connectWith: ".connectedSortable",
        items: "li:not(.unsortable)",
        placeholder: "ghost-highlight",
        revert: 200,
        tolerance: "intersect",
        over: function (event, ui) {
            if ($(ui.item).hasClass('manager-selection')) {
                $('div.applications').children('ul')
                    .children('li.ghost-highlight')
                    .removeClass('ghost-highlight')
                    .attr('style', 'height:0px; padding:0px; border: none;');
                $('div.available-apps').children('div').children('div.applications').addClass('warning-background');
                $('div.warning').show();
            }
            else {
                if ($('.positions').children('div.ghost-box')) {
                    $('.positions').children('div.ghost-box').fadeOut("normal", function () {
                        $(this).remove();
                    });
                }
            }
        },
        out: function (event, ui) {
            if ($(ui.item).hasClass('manager-selection')) {
                $('div.applications').removeClass('warning-background');
                $('div.warning').hide();
            }
        },
        receive: function (event, ui) {
            $(ui.item).removeAttr('style');
            if ($(ui.item).hasClass('manager-selection')) {
                $(ui.sender).sortable('cancel');
            }
            else {
                var nb = $('.positions').children('div').length - 1;
                $('.positions').children('div:last').fadeOut("normal", function () {
                    $(this).remove();
                });
            }

            //show empty message on sender if applicable
            $('li.emptyMessage', this).hide();
            if ($('li:not(.emptyMessage)', ui.sender).length == 0) {
                $('li.emptyMessage', ui.sender).show();
            }
            else {
                $('li.emptyMessage', ui.sender).hide();
            }
        },
        start: function (event, ui) {
            ui.placeholder.height(ui.item.height());
            $(ui.item).removeClass('grab');
            $(ui.item).addClass('grabbing');
        },
        stop: function (event, ui) {
            $(ui.item).removeClass('grabbing');
            $(ui.item).addClass('grab');
        }
    }).disableSelection();

    $("#sortable2").sortable({
        connectWith: [".connectedSortable", ".connectedToTrash"],
        dropOnEmpty: true,
        placeholder: "ghost-highlight",
        items: "li:not(.unsortable)",
        revert: 200,
        tolerance: "intersect",
        receive: function (event, ui) {
            $(ui.item).removeAttr('style').addClass('manager-selection');
            if (!$(ui.item).hasClass('perso')) {
                $(ui.item).addClass('perso')
            }
            $('.positions').children('div:last').remove();
            var nb = $('.positions').children('div').length + 1;
            $('<div><span>' + nb + '</span></div>').appendTo('.positions');

            //show empty message on sender if applicable
            $('li.emptyMessage', this).hide();
            if ($('li:not(.emptyMessage)', ui.sender).length === 0) {
                $('li.emptyMessage', ui.sender).show();
            }
            else {
                $('li.emptyMessage', ui.sender).hide();
            }
        },
        start: function (event, ui) {
            ui.placeholder.height(ui.item.height());
            $(ui.item).removeClass('grab');
            $(ui.item).addClass('grabbing');
        },

        stop: function (event, ui) {
            $(ui.item).removeClass('grabbing');
            $(ui.item).addClass('grab');
        },

        over: function (event, ui) {
            if ($('ul#sortable2').children('li.emptyMessage').is(':visible')) {
                $('ul#sortable2').children('li.emptyMessage').hide();
            }
            if (!$(ui.item).hasClass('manager-selection')) {
                var nb = $('.positions').children('div').length + 1;
                $('<div class="ghost-box" style="opacity:0.7; border:1px dashed;"><span>' + nb + '</span></div>').appendTo('.positions');
            }
        },
        out: function (event, ui) {
            if ($('.positions').children('div:last').hasClass('ghost-box')) {
                $('.positions').children('div:last').remove();
                if ($('.positions').children('div').length === 0) {
                    if (!$('ul#sortable2').children('li.emptyMessage').is(':visible')) {
                        $('ul#sortable2').children('li.emptyMessage').show();
                    }
                }
            }
        }
    }).disableSelection();
    $("#sortable3").sortable({
        connectWith: ".connectedToTrash",
        dropOnEmpty: true,
        placeholder: "ghost-highlight",
        items: "li:not(.unsortable)",
        revert: 200,
        tolerance: "intersect",
        receive: function (event, ui) {
            $(ui.item).removeAttr('style').removeClass('manager-selection');
            $(ui.item).children('div.oblig').remove();
            var nb = $('.positions').children('div').length - 1;
            $('.positions').children('div:last').fadeOut("normal", function () {
                $(this).remove();
            });
            //hide empty message on receiver
            $('li.emptyMessage', this).hide();
            //show empty message on sender if applicable
            if ($('li:not(.emptyMessage)', ui.sender).length == 0) {
                $('li.emptyMessage', ui.sender).show();
            }
            else {
                $('li.emptyMessage', ui.sender).hide();
            }
        },
        over: function (event, ui) {
            if ($('ul#sortable3').children('li.emptyMessage').is(':visible')) {
                $('ul#sortable3').children('li.emptyMessage').hide();
            }
        },
        start: function (event, ui) {
            ui.placeholder.height(ui.item.height());
            $(ui.item).removeClass('grab');
            $(ui.item).addClass('grabbing');
        },
        stop: function (event, ui) {
            $(ui.item).removeClass('grabbing');
            $(ui.item).addClass('grab');
        }
    }).disableSelection();
    getAvailableApplications();
});