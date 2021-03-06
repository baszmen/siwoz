﻿$(function () {
    $(".post-link").each(function () {
        postLink.call(this, false);
    });
    $(".post-link-confirm").each(function () {
        postLink.call(this, true);
    });

    var d = new Date();
    d.setHours(d.getHours() + 3);
    d.setMinutes(0);
    var dStr = d.toISOString().slice(0, 16).replace("T", " ");
    $('.datetimepicker').not('.date').datetimepicker({
        lang: 'pl',
        timepicker: true,
        step: 15,
        defaultTime: d.toISOString().slice(11, 16)
    });
    $('.datetimepicker.date').datetimepicker({
        lang: 'pl',
        timepicker: false,
        format: 'Y-m-d',
        onChangeDateTime: function (dp, $input) {
            if ($input.val() != "")
                window.location = window.location.href.split('?')[0] + "?visitsDate=" + $input.val();
        }
    });
    $(".datetimepicker[value='']").val(dStr);
    //tooltips();
});

function tooltips() {
    $(document).tooltip({
        items: "[data-user-tooltip]",
        tooltipClass: "user-tooltip",
        track: true,
        position: {
            my: "bottom+100",
            collision: "flipfit"
        },
        content: function () {
            var element = $(this);
            var url = element.attr("data-user-tooltip");
            return '<img class="user-img" src="' + url + '">';
        }
    });
}

/**
Creates form with post submit from link.
If confirm is true, confirm dialog is being shown before submit.
*/
function postLink(confirm) {
    var $this = $(this).clone(),
        href = $this.attr("href"),
        confirmText = $this.attr("data-confirm-text"),
        $form,
        $confirmDialog,
        $html;

    $form = $("<form method='post'></form>").attr("action", href);
    $this.attr("href", "");

    $form.append($this);
    $(this).replaceWith($form);

    if (confirm) {
        $confirmDialog = $("<div></div>").html(confirmText);
        $confirmDialog.dialog({
            modal: true,
            zIndex: 10,
            autoOpen: false,
            resizable: false,
            buttons: {
                Yes: function () {
                    $form.submit();
                    $(this).dialog("close");
                },
                No: function () {
                    $(this).dialog("close");
                }
            },
        });
    }

    $this.click(function (e) {
        e.preventDefault();

        if (confirm) {
            $confirmDialog.dialog("open");
        }
        else {
            $form.submit();
        }
        return false;
    });

}
