# NearBuy

A mini local marketplace where users can post, browse, and comment on secondhand or unused items — built as a university team web project.

# What it does

NearBuy lets people list items they want to sell (furniture, electronics, textbooks, anything) and lets others browse, search, and ask questions directly on the listing via comments — no messaging system, no accounts required just to browse.

Core loop: post an item → someone finds it → they comment to ask about it → deal happens outside the app.

# Pages
Listings (home) — browse all active listings, filter by category, search by keyword  
Listing detail — full item view (photos, price, description, seller, comments)  
Post a listing — create a new listing (logged-in users)  
My listings — manage your own posts: edit, delete, mark as sold  
Login / Register — basic auth  

# Branching workflow
main — stable, demo-ready state  
dev — active integration branch  
feature/<short-desc> — one branch per task, merged into dev via reviewed MR  
