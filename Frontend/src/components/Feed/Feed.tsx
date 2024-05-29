// src/components/Feed.js
import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import React from "react";

const Feed = () => (
	<Container>
		<Typography variant='h4' component='h2' gutterBottom>
			Feed
		</Typography>
		<Typography paragraph>This is where the feed content will be displayed.</Typography>
	</Container>
);

export default Feed;
