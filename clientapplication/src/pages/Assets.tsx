import Navbar from "../components/Navbar";
import CollapsedFileList from "../components/CollapsedFileList";
import { useEffect, useState } from "react";

const BASE_URL = "https://localhost:5173"; 

interface AssetInfo {
    id: number;
    name: string;
    description: string
}

interface ItemInfo {
    name: string;
    content: string
}

function Assets() {
    const header: string = "Available assets";

    const [isLoading, setIsLoading] = useState(false);
    const [items, setItems] = useState<ItemInfo[]>([]);

    useEffect(() => {
        const fetchAssetsInfo = async () => {
            if (items.length > 1)
                return;

            setIsLoading(true);

            try {
                const response = await fetch(`${BASE_URL}/inputasset/`);
                const assets = (await response.json()) as AssetInfo[];
                const mappedItems = assets.map(({ name, description }) => {
                    return { name: name, content: description }
                });
                setItems(mappedItems);
            } catch (e: any) {
                console.log(e);
            } finally {
                setIsLoading(false);
            }
        };

        fetchAssetsInfo();
    }, [isLoading, items]);


    return (
        <>
            <Navbar />
            {isLoading && <div>Loading...</div>}
            {!isLoading && (<CollapsedFileList items={items} header={header} />)}
        </>      
  );
}

export default Assets;