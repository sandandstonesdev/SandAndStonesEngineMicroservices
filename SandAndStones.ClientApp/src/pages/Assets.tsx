import Navbar from "../components/Navbar";
import CollapsedFileList from "../components/CollapsedFileList";
import { useEffect, useState } from "react";
import { InputAssetBatch } from "../types/InputAssetBatch";

const BASE_URL = "https://localhost:5173"; 
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
                const response = await fetch(`${BASE_URL}/assetBatch/0`);
                const inputAssetBatch = (await response.json()) as InputAssetBatch;

                const mappedItems = inputAssetBatch.assets.map(({ name, ...rest }) => {
                    return {
                        name: name,
                        content: JSON.stringify(rest, null, 2)
                    }
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