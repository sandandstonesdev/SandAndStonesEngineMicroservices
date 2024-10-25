const sectionStyle = {
    width: 400,
    height: 400
};


function About() {
    const screenImageUrl = new URL('./SandAndStonesEngineScreen.png', import.meta.env.VITE_STATIC_IMAGES_URL).href
    
    return (
        <>
            <p>SandAndStonesEngine Screen</p>
            <div className="row">
                <div className="logo">
                    <img className="img-fluid" style={sectionStyle} src={screenImageUrl}></img>
                </div>
            </div>
        </>
  );
}

export default About;