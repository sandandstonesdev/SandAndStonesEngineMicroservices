import InputAsset from "../components/InputAsset.tsx";
import InputTexture from "../components/InputTexture.tsx";
import LogoutLink from "../components/LogoutLink.tsx";
import AuthorizedView, { AuthorizedUser } from "../components/AuthorizedView.tsx";

function Home() {
    return (
        <div>
        <p>SandAndStones Management Website</p>
        <AuthorizedView>
            <span><LogoutLink>Logout <AuthorizedUser value="email" /></LogoutLink></span>
            <InputAsset />
            <InputTexture />
            </AuthorizedView>
        </div>
    );
}

export default Home;