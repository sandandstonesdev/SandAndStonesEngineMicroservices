import { isAxiosError } from "axios";
import { useState } from "react";
import { axiosInstance } from "../hooks/useAxios";

type UploadedTextureProps = {
    header: string
}

function UploadTexture({ header }: UploadedTextureProps) {
    const [file, setFile] = useState<File | null>(null);
    const [fileLoaded, setFileLoaded] = useState(false);
    const [error, setError] = useState("");

    const onFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const files = e.currentTarget.files;
        if (files) {
            setFile(files[0])
            setFileLoaded(true);
        }
    };

    const handleFileSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        if (!fileLoaded)
            return;

        setError("");

        const reader = new FileReader();
        reader.readAsDataURL(file!);
        reader.onloadend = async () => {
            const base64data = reader.result?.toString().split(',')[1];

            const payload = {
                name: file!.name,
                base64Data: base64data
            };

            try {

                const response = await axiosInstance.post(
                    `${import.meta.env.VITE_APP_BASE_URL}/texture-api/upload`,
                    payload,
                    {
                        headers: {
                            "Content-Type": "application/json",
                            "Accept": "application/json",
                        }
                    }
                );
                console.log(response);
            } catch (err: unknown) {
                if (isAxiosError(err)) {
                    if (err.response) {
                        setError(`Error while Uploading Image: ${err.response.status} ${err.response.statusText}`);
                    }
                    else {
                        setError(`Error while Uploading Image: ${err.message}`);
                        console.error(err);
                    }
                }
            }
        }
    }

    return (
        <>
            <div>
                <div>
                    <h2>{header}</h2>
                </div>
                <div className={"texture-form"}>
                    <form onSubmit={handleFileSubmit}>
                        <input type="file" onChange={onFileChange}></input>
                        <input type="submit" value="Upload Image"></input>
                    </form>
                    <div className={"loaded-image"}>
                        {fileLoaded && <img src={file ? URL.createObjectURL(file) : ""} width="100" height="100" />}
                    </div>
                    {error && <p className="error">{error}</p>}
                </div>
            </div>
        </>
  );
}

export default UploadTexture;