// Pignose Calender Plugin Call
$(function () {
  $('.calendar').pignoseCalendar({
    scheduleOptions: {
      colors: {
        offer: '#009c8a',
        ad: '#F1635F'
      }
    },
    schedules: [{
      name: 'offer',
      date: '2018-11-08'
    }, {
      name: 'ad',
      date: '2018-11-09'
    }, {
      name: 'offer',
      date: '2018-11-30',
    }],
    select: function (date, context) {
      console.log('events for this date', context.storage.schedules);
    }
  });
  $('.calendar2').pignoseCalendar({ disabledWeekdays: [0, 6] });
  $('.calendar3').pignoseCalendar();
});