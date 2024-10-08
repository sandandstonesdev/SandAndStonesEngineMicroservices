import Navbar from "../components/Navbar";
import CollapsedFileList from "../components/CollapsedFileList";
import { useEffect, useState } from "react";
import { InputAssetBatch } from "../types/InputAssetBatch";

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
                const response = await fetch(`${import.meta.env.VITE_APP_BASE_URL}/assetBatch/0`);
                const inputAssetBatch = (await response.json()) as InputAssetBatch;
                const mappedItems = inputAssetBatch.assets.map(({ name, animationTextureFiles }) => {
                    return {
                        name: name,
                        textureNames: animationTextureFiles,
                        content: JSON.stringify(animationTextureFiles),
                        textures: 0
                    }
                });

                const mappedItemsWithImages = mappedItems.map(({ name, textureNames }) => {
                    const imageUrl = `${import.meta.env.VITE_APP_BASE_URL}/textureFile/${textureNames[0]}`;
                    return {
                        name: name,
                        content: <>
                            <div>
                                <img src={imageUrl} alt={textureNames[0]} />
                            </div >
                        </>
                    }
                });
                
                setItems(mappedItemsWithImages);
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
            <Navbar />
            {isLoading && <div>Loading...</div>}
            {!isLoading && (<CollapsedFileList items={items} header={header} />)}
        </>      
    );
}

export default Textures;