/*
* Calendar JavaScript Library v3.0.0
*
* Copyright 2013-2014, Steven
*
* Date: 2014/08/28
*/
var CompanyX = {
    getCalendar: function() { return CompanyX.RightNavCalendarPanel; },
    getStore: function() { return CompanyX.CalendarEventStore; },
    getWindow: function() { return CompanyX.CalendarEventEditWindow; },
    viewChange: function(p, vw, dateInfo) {
        var win = this.getWindow();
        if (win) { win.hide(); }
        if (dateInfo !== null) {
            this.CalendarDatePicker.setValue(dateInfo.activeDate);
            this.updateTitle(dateInfo.viewStart, dateInfo.viewEnd);
        }
    },
    updateTitle: function(startDt, endDt) {
        var msg = "";
        if (startDt.clearTime().getTime() == endDt.clearTime().getTime()) { msg = startDt.format("Y/m/d"); } else { msg = startDt.format("Y/m/d") + " - " + endDt.format("Y/m/d"); }
        this.MainPanel.setTitle(msg);
    },
    setStartDate: function(picker, date) {
        this.getCalendar().setStartDate(date);
    },
    rangeSelect: function(cal, dates, callback) {
        this.record.show(cal, dates);
        this.getWindow().on("hide", callback, cal, { single: true });
    },
    dayClick: function(cal, dt, allDay, el) {
        this.record.show.call(this, cal, {
            StartDate: dt,
            IsAllDay: allDay
        }, el);
    },
    record: {
        add: function(win, rec) {
            win.hide();
            X.CalendarManager.AddRecord(Ext.encode(rec.data), {
                eventMask: {
                    showMask: true,
                    msg: LanguageSet.Saving,
                    target: "customtarget",
                    customTarget: this.getCalendar()
                }
            });
        },
        update: function(win, rec) {
            win.hide();
            X.CalendarManager.UpdateRecord(Ext.encode(rec.data), {
                eventMask: {
                    showMask: true,
                    msg: LanguageSet.Updating,
                    target: "customtarget",
                    customTarget: this.getCalendar()
                }
            });
        },
        remove: function(win, rec) {
            win.hide();
            X.CalendarManager.DeleteRecord(Ext.encode(rec.data), {
                eventMask: {
                    showMask: true,
                    msg: LanguageSet.Deleting,
                    target: "customtarget",
                    customTarget: this.getCalendar()
                }
            });
        },
        edit: function(win, rec) {
            win.hide();
            CompanyX.getCalendar().showEditForm(rec);
        },
        addDetail: function(cal, rec) {
            X.CalendarManager.AddRecord(Ext.encode(rec.data), {
                eventMask: {
                    showMask: true,
                    msg: LanguageSet.Saving,
                    target: "customtarget",
                    customTarget: this.getCalendar()
                }
            });
        },
        updateDetail: function(cal, rec) {
            X.CalendarManager.UpdateRecord(Ext.encode(rec.data), {
                eventMask: {
                    showMask: true,
                    msg: LanguageSet.Updating,
                    target: "customtarget",
                    customTarget: this.getCalendar()
                }
            });
        },
        removeDetail: function(cal, rec) {
            X.CalendarManager.DeleteRecord(Ext.encode(rec.data), {
                eventMask: {
                    showMask: true,
                    msg: LanguageSet.Deleting,
                    target: "customtarget",
                    customTarget: this.getCalendar()
                }
            });
        },
        resize: function(cal, rec) {
            X.CalendarManager.ResizeRecord(Ext.encode(rec.data), {
                eventMask: {
                    showMask: true,
                    msg: LanguageSet.Updating,
                    target: "customtarget",
                    customTarget: this.getCalendar()
                }
            });
        },
        move: function(cal, rec) {
            X.CalendarManager.MoveRecord(Ext.encode(rec.data), {
                eventMask: {
                    showMask: true,
                    msg: LanguageSet.Updating,
                    target: "customtarget",
                    customTarget: this.getCalendar()
                }
            });
        },
        show: function(cal, rec, el) {
            CompanyX.getWindow().show(rec, el);
        },
        saveAll: function() {
            CompanyX.getStore().submitData();
        }
    },
    doRemind: function() {
        var store = CompanyX.getStore(),
        rem,
        interval,
        M = Ext.calendar.EventMappings;
        store.each(function(r) {
            rem = r.data[M.Reminder.name];
            if (!Ext.isEmpty(rem)) {
                rem = parseInt(rem) * 60 * 1000;
                interval = r.data[M.StartDate.name].getTime() - (new Date()).getTime();
                if (interval >= 0 && interval <= rem) {
                    X.CalendarManager.ShowReminder(r.data[M.Title.name], r.data[M.StartDate.name].format("Y/m/d H:i"));
                }
            }
        });
    },
    localize: {
        editWin: function(el) {
            el.fbar.items.get(0).text = String.format('<a href="#" id="tblink">{0}</a>', LanguageSet.EditDetail);
            el.fbar.items.get(2).text = LanguageSet.Save;
            el.fbar.items.get(3).text = LanguageSet.Delete;
            el.fbar.items.get(4).text = LanguageSet.Cancel;
            var titleItem = el.formPanel.get("title"),
            dateRangeItem = el.formPanel.get("date-range"),
            calendarItem = el.formPanel.get("calendar");
            titleItem.fieldLabel = LanguageSet.Title;
            dateRangeItem.fieldLabel = LanguageSet.Date;
            dateRangeItem.toText = "~";
            dateRangeItem.allDayText = LanguageSet.DateRange;
            calendarItem.fieldLabel = LanguageSet.Calendar;
        },
        editForm: function(el) {
            var form = el.get(el.id + "-edit");
            form.fbar.items.get(0).text = LanguageSet.Save;
            form.fbar.items.get(1).text = LanguageSet.Delete;
            form.fbar.items.get(2).text = LanguageSet.Cancel;

            var titleItem = form.get("left-col").items.get(0),
            dateRangeItem = form.get("left-col").items.get(1),
            calendarItem = form.get("left-col").items.get(2),
            reminderItem = form.get("left-col").items.get(3),
            notesItem = form.get("right-col").items.get(0),
            locationItem = form.get("right-col").items.get(1),
            weblinkItem = form.get("right-col").items.get(2);

            titleItem.fieldLabel = LanguageSet.Title;
            dateRangeItem.fieldLabel = LanguageSet.Date;
            dateRangeItem.toText = "~";
            dateRangeItem.allDayText = LanguageSet.DateRange;
            calendarItem.fieldLabel = LanguageSet.Calendar;
            reminderItem.fieldLabel = LanguageSet.Reminder;
            notesItem.fieldLabel = LanguageSet.Notes;
            locationItem.fieldLabel = LanguageSet.Location;
            weblinkItem.fieldLabel = LanguageSet.WebLink;
        }
    }
};