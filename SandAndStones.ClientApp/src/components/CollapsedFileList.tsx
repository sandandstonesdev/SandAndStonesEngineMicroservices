import { useState } from "react";

interface CollapsedItem {
    name: string; content: any
}

export type CollapsedProps = {
    header: string
    items: Array<CollapsedItem>;
}

function CollapsedFileList({ items, header }: CollapsedProps) {

    const [selected, setSelected] = useState(null);
    const toggle = (i: any) => {
        if (selected == i) {
            return setSelected(null);
        }

        setSelected(i);
    }

    return (
        <>
            <h2>{header}</h2>
            <div className="accordion">
                {items.map((item, i) => (
                    <div key={item.name} className="item">
                        <div className="title" onClick={() => toggle(i)}>
                            <h3>{item.name}</h3>
                            <span>{selected === i ? '-' : '+'}</span>
                        </div>
                        <div className={
                            selected === i ? "content show" : "content hide"
                        }>
                            <div>{item.content}</div>
                        </div>
                    </div>
                ))}
            </div>
        </>
    );
}

export default CollapsedFileList;