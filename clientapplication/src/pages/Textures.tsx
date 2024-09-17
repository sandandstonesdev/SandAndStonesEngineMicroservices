import Navbar from "../components/Navbar";
import CollapsedFileList from "../components/CollapsedFileList";

function Textures() {
    const items = [
        { name: "Item1", content: "Bla1 bla1 bla1" },
        { name: "Item2", content: "Bla2 bla2 bla2" },
        { name: "Item3", content: "Bla3 bla3 bla3" }
    ];

    const header: string = "Available textures";

    return (
        <>
            <Navbar />
            <CollapsedFileList items={items} header={header} />
        </>
    );
}

export default Textures;