import Navbar from "../components/Navbar";
import CollapsedFileList from "../components/CollapsedFileList";
import { useEffect, useState } from "react";

const BASE_URL = "https://localhost:5173"; 

interface AssetInfo {
    id: number;
    name: string;
    description: string
}

interface ImageItemInfo {
    name: string;
    content: JSX.Element;
}

function Textures() {
    const header: string = "Available textures";

    const [isLoading, setIsLoading] = useState(false);
    const [items, setItems] = useState<ImageItemInfo[]>([]);

    useEffect(() => {
        const fetchAssetsInfo = async () => {
            if (items.length > 1)
                return;

            setIsLoading(true);

            try {
                const response = await fetch(`${BASE_URL}/inputasset/`);
                const assets = (await response.json()) as AssetInfo[];
                const imageUrl = `${BASE_URL}/texture/`;
                const mappedItems = assets.map(({ name }) => {
                    return {
                        name: name, content: <>
                            <div>
                                <img src={imageUrl} alt="wall.png" />
                            </div > </> }
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

export default Textures;