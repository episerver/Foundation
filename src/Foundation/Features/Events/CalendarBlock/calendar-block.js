import { Calendar } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import listPlugin from '@fullcalendar/list';

export default class CalendarBlock {
    init() {
        if (document.querySelector(".calendar-block") == null) {
            return;
        }

        let calendarBlocks = document.querySelectorAll(".calendar-block");
        calendarBlocks.forEach((item, index) => {
            let url = window.location.origin;
            if (item.dataset.blockViewmode === "Upcoming") {
                url += "/CalendarBlock/UpcomingEvents";
            }
            else {
                url += "/CalendarBlock/CalendarEvents";
            }

            let data = {
                blockId: item.dataset.blockId,
            };

            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
            })
                .then(response => response.json())
                .then(data => {
                    let blocks = document.querySelectorAll(".calendarblock");
                    blocks.forEach((obj, index) => {
                        let calendar = new Calendar(obj.children[0], {
                            plugins: [dayGridPlugin, listPlugin],
                            initialView: item.dataset.blockViewmode,
                            events: data,
                        });

                        calendar.render();
                    });
                })
                .catch((error) => {
                    console.error('Error:', error);
                });
        });
    }
}