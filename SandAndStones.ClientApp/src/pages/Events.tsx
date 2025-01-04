import { useEffect, useState } from "react";
import { axiosInstance } from "../hooks/useAxios";
import { EventItem } from "../types/EventItem";
import EventItemsList from "../components/EventItemsList";

function Events() {
    const eventsHeader: string = "Event List";
    const [isLoading, setIsLoading] = useState(false);
    const [eventItems, setEventItems] = useState<EventItem[]>([]);

    useEffect(() => {
        const fetchEventItems = async () => {
            setIsLoading(true);

            try {
                const response = await axiosInstance.get(`${import.meta.env.VITE_APP_BASE_URL}/eventlog-api/eventList`);
                const eventItems = await response.data as EventItem[];

                setEventItems(eventItems);
            } catch (e: unknown) {
                console.log(e);
            }
            finally {
                setIsLoading(false);
            }
        };

        fetchEventItems();
    }, []);

    return (
        <>
            <h2>Events</h2>
            {isLoading && <div>Loading...</div>}
            {!isLoading &&<EventItemsList items={eventItems} header={eventsHeader} / >}
        </>
    );
}

export default Events;