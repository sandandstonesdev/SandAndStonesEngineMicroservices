export interface InputAsset {
    id: number;
    name: string;
    instances: number;
    startPos: number[];
    endPos: number[];
    instancePosOffset: number[];
    assetBatchType: string;
    assetType: string;
    color: number[];
    text: string
    animationTextureFiles: string[];
    depth: number;
    scale: number;
}