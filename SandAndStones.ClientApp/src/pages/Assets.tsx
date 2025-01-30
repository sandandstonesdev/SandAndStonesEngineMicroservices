import CollapsedFileList from "../components/CollapsedFileList";
import { useEffect, useState } from "react";
import { AssetBatch } from "../types/AssetBatch";
import { axiosInstance } from "../hooks/useAxios";
 
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
                const response = await axiosInstance.get(`${import.meta.env.VITE_APP_BASE_URL}/asset-api/assetBatch/0`);
                const assetBatch = await response.data as AssetBatch;

                const mappedItems = assetBatch.assets.map(({ name, ...rest }) => {
                    return {
                        name: name,
                        content: JSON.stringify(rest, null, 2)
                    }
                });

                setItems(mappedItems);
            } catch (e: unknown) {
                console.log(e);
            } finally {
                setIsLoading(false);
            }
        };

        fetchAssetsInfo();
    }, [items]);


    return (
        <>
            {isLoading && <div>Loading...</div>}
            {!isLoading && (<CollapsedFileList items={items} header={header} />)}
        </>      
  );
}

export default Assets;