import "./EventItemsList.css";
import { EventItem } from "../types/EventItem";

type EventItems = {
    header: string
    items: Array<EventItem>;
}

function EventItemsList({ items, header }: EventItems) {
    return (
        <>
            <h2>{header}</h2>
            <div className="event-items-container">
                {items.map((item) => (
                    <div key={item.id} className="event-item-card">
                        <div className="event-item-row">
                            <span className="event-item-label">ID:</span>
                            <span className="event-item-value">{item.id}</span>
                        </div>
                        <div className="event-item-row">
                            <span className="event-item-label">Resource ID:</span>
                            <span className="event-item-value">{item.resourceId}</span>
                        </div>
                        <div className="event-item-row">
                            <span className="event-item-label">Resource Name:</span>
                            <span className="event-item-value">{item.resourceName}</span>
                        </div>
                        <div className="event-item-row">
                            <span className="event-item-label">Date Time:</span>
                            <span className="event-item-value">{item.dateTime.toLocaleString()}</span>
                        </div>
                        <div className="event-item-row">
                            <span className="event-item-label">Current User ID:</span>
                            <span className="event-item-value">{item.currentUserId}</span>
                        </div>
                    </div>
                ))}
            </div>
        </>
  );
}

export default EventItemsList;