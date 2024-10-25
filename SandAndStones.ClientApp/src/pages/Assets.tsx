import CollapsedFileList from "../components/CollapsedFileList";
import { useEffect, useState } from "react";
import { InputAssetBatch } from "../types/InputAssetBatch";
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
                const response = await axiosInstance.get(`${import.meta.env.VITE_APP_BASE_URL}/assetBatch/0`);
                const inputAssetBatch = response.data as InputAssetBatch;

                const mappedItems = inputAssetBatch.assets.map(({ name, ...rest }) => {
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
    }, [isLoading, items]);


    return (
        <>
            {isLoading && <div>Loading...</div>}
            {!isLoading && (<CollapsedFileList items={items} header={header} />)}
        </>      
  );
}

export default Assets;