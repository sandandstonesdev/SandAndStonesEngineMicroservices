import CollapsedFileList from "../components/CollapsedFileList";
import { useEffect, useState } from "react";
import { InputAssetBatch } from "../types/InputAssetBatch";
import { axiosInstance } from "../hooks/useAxios";
import UploadTexture from "../components/UploadTexture";

interface ImageItemInfo {
    name: string;
    content: JSX.Element;
}

function Textures() {
    const uploadTextureHeader: string = "Upload Texture";
    const availableTexturesHeader: string = "Available Textures";

    const [isLoading, setIsLoading] = useState(false);
    const [items, setItems] = useState<ImageItemInfo[]>([]);

    useEffect(() => {
        const fetchAssetsInfo = async () => {
            if (items.length > 1)
                return;

            setIsLoading(true);

            try {
                const response = await axiosInstance.get(`${import.meta.env.VITE_APP_BASE_URL}/asset-api/assetBatch/0`);
                const inputAssetBatch = response.data as InputAssetBatch;
                const mappedItems = inputAssetBatch.assets.map(({ name, animationTextureFiles, text }) => {
                    return {
                        name: name,
                        textureNames: animationTextureFiles,
                        content: JSON.stringify(animationTextureFiles),
                        textures: 0,
                        isDynamic: !(text.trim() == '')
                    }
                });

                const mappedItemsWithImages = mappedItems.map(({ name, textureNames, isDynamic }) => {
                    const imageUrl = `${import.meta.env.VITE_APP_BASE_URL}/texture-api/textureFile/${textureNames[0]}`;
                    return {
                        name: name,
                        content: <>
                            {!isDynamic ?
                                <div>
                                <img src={imageUrl} alt={textureNames[0]} />
                                </div>
                                :
                                <div>
                                    <p>This is dynamic (procedural) texture in game. You cannot get it!</p>
                                </div >
                            }
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
    }, [items]);

    return (
        <>
            <UploadTexture header={uploadTextureHeader}></UploadTexture>
            {isLoading && <div>Loading...</div>}
            {!isLoading && (<CollapsedFileList items={items} header={availableTexturesHeader} />)}
        </>      
    );
}

export default Textures;