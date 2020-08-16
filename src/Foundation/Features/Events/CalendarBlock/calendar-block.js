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
            let url = "";
            if (item.dataset.blockViewmode === "Upcoming") {
                url = "CalendarBlock/UpcomingEvents";
            }
            else {
                url = "CalendarBlock/CalendarEvents";
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
                    let currentBlock = document.querySelector(`#calendar-block-${item.dataset.blockId}`);
                    let calendar = new Calendar(currentBlock, {
                        plugins: [dayGridPlugin, listPlugin],
                        initialView: item.dataset.blockViewmode,
                        events: data,
                    });

                    calendar.render();
                })
                .catch((error) => {
                    console.error('Error:', error);
                });
        });
    }
}