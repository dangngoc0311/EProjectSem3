using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryOnline.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    BannerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BannerTitle = table.Column<string>(maxLength: 150, nullable: false),
                    Image = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Status = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.BannerId);
                });

            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    ConfigId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    Address = table.Column<string>(maxLength: 150, nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => x.ConfigId);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    CouponId = table.Column<string>(nullable: false),
                    CouponName = table.Column<string>(maxLength: 150, nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.CouponId);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<string>(nullable: false),
                    PaymentName = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<string>(nullable: false),
                    ServiceName = table.Column<string>(maxLength: 150, nullable: false),
                    ServiceDescription = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(maxLength: 80, nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    Address = table.Column<string>(maxLength: 150, nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    Role = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitId = table.Column<string>(nullable: false),
                    UnitName = table.Column<string>(maxLength: 150, nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    ServiceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_Units_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Content = table.Column<string>(type: "ntext", nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.BlogId);
                    table.ForeignKey(
                        name: "FK_Blogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(maxLength: 80, nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    Address = table.Column<string>(maxLength: 150, nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    PaymentStatus = table.Column<byte>(nullable: false),
                    OrderStatus = table.Column<byte>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    PaymentId = table.Column<string>(nullable: false),
                    CouponId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupons",
                        principalColumn: "CouponId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsedCoupons",
                columns: table => new
                {
                    UsedCouponId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    CouponId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedCoupons", x => x.UsedCouponId);
                    table.ForeignKey(
                        name: "FK_UsedCoupons_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupons",
                        principalColumn: "CouponId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsedCoupons_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PriceUnit = table.Column<double>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    OrderId = table.Column<string>(nullable: true),
                    UnitId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_UnitId",
                table: "OrderItems",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CouponId",
                table: "Orders",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentId",
                table: "Orders",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_ServiceId",
                table: "Units",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedCoupons_CouponId",
                table: "UsedCoupons",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedCoupons_UserId",
                table: "UsedCoupons",
                column: "UserId");

            // Insert data Banners
            migrationBuilder.InsertData(
               table: "Banners",
               columns: new[] { "BannerId", "BannerTitle", "Image", "CreatedAt", "Status"},
               values: new object[,]{
                    { 1,"Laundry Services","mainslide02-01.jpg","2022-11-25 00:00:00.0000000",1},
                    { 2,"Laundry Services","services_img01.jpg","2022-11-25 00:00:00.0000000",1}
                });

            // Insert data Coupons
            migrationBuilder.InsertData(
              table: "Coupons",
              columns: new[] { "CouponId", "CouponName", "Discount", "Status", "CreatedAt","ExpirationDate" },
              values: new object[,]{
                    { "00daae60-ae37-46fb-82dd-3873ef93d09d","Christmas Discount",1.0,1,"2022-11-25 00:00:00.0000000","2022-12-28 00:00:00.0000000"},
                    { "50f665f5-70eb-4ae9-a0c6-8f6e0f282e50","Winter promotion",1.0,1,"2022-11-25 00:00:00.0000000","2022-12-30 00:00:00.0000000"},
                    { "e4e1d0cd-f636-4567-bfb2-40b244c504e5","New Member",1.0,1,"2022-11-25 00:00:00.0000000","2023-12-28 00:00:00.0000000"}
               });

            //Insert data Payment
            migrationBuilder.InsertData(
              table: "Payments",
              columns: new[] { "PaymentId", "PaymentName" },
              values: new object[,]{
                    { "0709196d-913e-45a7-9fd3-564d3737e9c7","Pay after receiving"},
                    { "f3e5fba5-d557-40f0-a9fd-1c66e9874c78","Banking"}
               });

            //Insert data Services
            migrationBuilder.InsertData(
              table: "Services",
              columns: new[] { "ServiceId", "ServiceName", "ServiceDescription" },
              values: new object[,]{
                    { "7bc16a9d-df3d-4d90-ba16-4488a9222a98","Package wise Invoice","<p>Select a package for a month</p>"},
                    { "7d20df94-6461-414e-ae90-3c2f737e341f","Weight wise Invoice","<p>A bunch of cloth weight wise.</p>"},
                    { "97f9e5c2-477a-40a2-8cb2-99b07806a792","Pieces wise Invoice","<p>Give clothes by piece wise.</p>"}
               });

            //Insert data Units
            migrationBuilder.InsertData(
               table: "Units",
               columns: new[] { "UnitId", "UnitName", "UnitPrice", "Image", "ServiceId" },
               values: new object[,]{
                    { "0717d50d-5572-4073-a032-27ee306767b7","Long Shirts",2.0,"service1.jpg","97f9e5c2-477a-40a2-8cb2-99b07806a792"},
                    { "21548867-05a5-41a7-823b-7c37ed14c613","Jeans",2.0,"unit6.jpg","97f9e5c2-477a-40a2-8cb2-99b07806a792"},
                    { "258bfbee-4054-48e9-9541-329477485bea","Bedsheets ( kg )",2.0,"layout01-img01.jpg","7d20df94-6461-414e-ae90-3c2f737e341f"},
                    { "3b3c0552-9721-48bd-bba0-99aaee3f74b0","Wind Clothes ( kg )",5.0,"e92355a70ae8f3c2919bb73c15773646.jpg","7d20df94-6461-414e-ae90-3c2f737e341f"},
                    { "6f6817a7-ae78-40af-b7b3-64195e6dc1de","Face Towel, Bath Towel ( kg )",3.0,"unit1.jpg","7d20df94-6461-414e-ae90-3c2f737e341f"},
                    { "a43b41fe-6d63-4553-9f22-48c810b8ce2f","The Dining Room",15.0,"blog4.png","7d20df94-6461-414e-ae90-3c2f737e341f"},
                    { "a443eef6-5f81-40d6-b64c-197d4e35a9cd","Bedroom Furniture ( combo )",20.0,"layout01-img01.jpg","7bc16a9d-df3d-4d90-ba16-4488a9222a98"},
                    { "bd20c41c-dfbc-457b-b0f2-e26de59f1aba","Vest",5.0,"blog7.jpg","97f9e5c2-477a-40a2-8cb2-99b07806a792"},
                    { "bf006310-72bd-45d9-a8cb-d80ceb5971a5","Vest + Suit ( combo )",10.0,"blog7.jpg","7bc16a9d-df3d-4d90-ba16-4488a9222a98"},
                    { "f3ece847-f6ab-41e2-9701-70149e7b29c5","Baby Tools ( kg )",2.0,"blog11.jpg","7d20df94-6461-414e-ae90-3c2f737e341f"},
                });

            // Insert data Config
            migrationBuilder.InsertData(
                table: "Config",
                columns: new[] { "ConfigId", "Description","EmailAddress","ContactNumber","Address","Status","Image" },
                values: new object[,]{
                    { "914a1622-6d3d-4415-8bc7-7960625e502e","<p>hihihiihihihih</p>","laundryonline@gmail.com","0987346532","Ha Noi" ,1,"logo1.png"}
                 });
           
            // Insert data Users
            migrationBuilder.InsertData(
               table: "Users",
               columns: new[] { "UserId", "FullName", "EmailAddress", "ContactNumber", "Address", "UserName", "Password", "Status", "Role" },
               values: new object[,]{
                    { "3ff3b26f-21d1-4518-88f2-6310d8fedc11","Laundry Online","admin@gmail.com","0394880635","Phuc Tho - Ha Noi" ,"admin","E10ADC3949BA59ABBE56E057F20F883E",1,0},
                    { "448073e7-c403-4a6d-93d5-eaba135ae0aa","Dang Minh Hoang","minhhoang140802@gmail.com","0583345469","Me Linh - Ha Noi" ,"minhhoang","E10ADC3949BA59ABBE56E057F20F883E",1,1},
                    { "97576b8c-2f9c-452b-b259-bd49174fdb71","Dang Thi Bich Ngoc","ngoc1233456789@gmail.com","0973461932","Soc Son - Ha Noi" ,"bichngoc","E10ADC3949BA59ABBE56E057F20F883E",1,1},
                });

            //  Insert data Blogs
            migrationBuilder.InsertData(
            table: "Blogs",
            columns: new[] { "BlogId", "Title", "Image", "Content", "Status", "CreatedAt", "UserId" },
            values: new object[,]{
                    { "191fdcdc-e0b4-4b12-a756-2861e00044e2","Getting Your Bedding Ready for Winter","blog10.jpg","<p>For clients of Pool Service and Pool Troopers there is still one month to take advantage of our Spring Special Filter maintenance and equipment check. This annual preventative maintenance on your pool&rsquo;s equipment is designed to get your pool and its equipment ready for the swim season which is upon us now. DE filters need to be disassembled and cleaned, and cartridges need to be checked and replaced if needed. Seals and O-rings need to be checked, lubricated and replaced, and a safety inspection of the pool pump and filter, the timer and wiring, as well as the drain covers in the pool needs to be done. All of this is included in this service.</p><p>Thousands of you have already taken advantage of this service where we bundle all these activities and give you a discount to perform these services. Of course we do these things year round, but in the spring when most clients need it we do it for a reduced price. We believe you should be free of the worries and concerns of your pool so that you are truly free to enjoy your pool.</p><p>That belief is why we discount our prices when you need the service the most. Give us a call today, this Spring Special runs through May, and we would be honored to have your back and get your pool ready for the family&rsquo;s enjoyment!</p>",1,"2022-11-25 00:00:00.0000000","3ff3b26f-21d1-4518-88f2-6310d8fedc11"},
                    { "502d01c6-cd6c-4837-bf39-1bc93e19edea","Keeping Your Bedroom Smelling Fresh","blog15.jpg","<p>I confess: it has been a problem most of my married life. I&rsquo;ve tried several ways of dealing with this &lsquo;problem&rsquo; and I could never find the right combination of events. What &lsquo;problem&rsquo;, you say? The boy smell. Ever walk in to a boy&rsquo;s college dorm room? Did you have a brother? Yes.</p>",1,"2022-11-25 00:00:00.0000000","3ff3b26f-21d1-4518-88f2-6310d8fedc11"},
                    { "54fe3648-1057-42cf-ae4f-41e10f0e3305","Follow-up to Washing Sheets & Towels","blog14.jpg","<p>For clients of Pool Service and Pool Troopers there is still one month to take advantage of our Spring Special Filter maintenance and equipment check. This annual preventative maintenance on your pool&rsquo;s equipment is designed to get your pool and its equipment ready for the swim season which is upon us now. DE filters need to be disassembled and cleaned, and cartridges need to be checked and replaced if needed. Seals and O-rings need to be checked, lubricated and replaced, and a safety inspection of the pool pump and filter, the timer and wiring, as well as the drain covers in the pool needs to be done. All of this is included in this service.</p><p>Thousands of you have already taken advantage of this service where we bundle all these activities and give you a discount to perform these services. Of course we do these things year round, but in the spring when most clients need it we do it for a reduced price. We believe you should be free of the worries and concerns of your pool so that you are truly free to enjoy your pool.</p><p>That belief is why we discount our prices when you need the service the most. Give us a call today, this Spring Special runs through May, and we would be honored to have your back and get your pool ready for the family&rsquo;s enjoyment!</p>",1,"2022-11-25 00:00:00.0000000","3ff3b26f-21d1-4518-88f2-6310d8fedc11"},
                    { "6a80b8b2-c713-4ba4-9696-1cbbd31725a2","Washing and Care Instructions for Table Linens 1","blog13.jpg","<p>For clients of Pool Service and Pool Troopers there is still one month to take advantage of our Spring Special Filter maintenance and equipment check. This annual preventative maintenance on your pool&rsquo;s equipment is designed to get your pool and its equipment ready for the swim season which is upon us now. DE filters need to be disassembled and cleaned, and cartridges need to be checked and replaced if needed. Seals and O-rings need to be checked, lubricated and replaced, and a safety inspection of the pool pump and filter, the timer and wiring, as well as the drain covers in the pool needs to be done. All of this is included in this service.</p><p>Thousands of you have already taken advantage of this service where we bundle all these activities and give you a discount to perform these services. Of course we do these things year round, but in the spring when most clients need it we do it for a reduced price. We believe you should be free of the worries and concerns of your pool so that you are truly free to enjoy your pool.</p><p>That belief is why we discount our prices when you need the service the most. Give us a call today, this Spring Special runs through May, and we would be honored to have your back and get your pool ready for the family&rsquo;s enjoyment!</p>",1,"2022-11-25 00:00:00.0000000","3ff3b26f-21d1-4518-88f2-6310d8fedc11"},
                    { "7cd63ac8-cafe-42a9-926e-1dfa5b37d7ad","Washing and Care Instructions for Table Linens 2","blog13.jpg","<p>Are you a family that uses tablecloths and cloth napkins often? We mainly use them around the holiday season here at the Hill House. I think people (meaning me) often don&rsquo;t use nicer cloths and napkins because we perceive it as work to get them ready for an event and then to get them clean after said event.Are you a family that uses tablecloths and cloth napkins often? We mainly use them around the holiday season here at the Hill House.</p><ul><li><em>Since people tend to use cloth napkins and tablecloths during the holiday season, I&rsquo;ve written several posts around that. Make sure you bookmark these and return to them in mid-late October. You&rsquo;ll be so happy you already have the post bookmarked and can find it easily when you&rsquo;re wondering how to remove that awful stain or how to prepare yourself for the holiday parties you&rsquo;ll host. Preparing Your Table Linens for Holiday Parties &ndash; This post includes ideas and a small planning guide to prevent you from waiting until the last minute to get your holiday linens ready.</em></li></ul><p><strong><em>&ndash; Michael M. Soderquist</em></strong></p><p>I think people (meaning me) often don&rsquo;t use nicer cloths and napkins because we perceive it as work to get them ready for an event and then to get them clean after said event.Are you a family that uses tablecloths and cloth napkins often? We mainly use them around the holiday season here at the Hill House. I think people (meaning me) often don&rsquo;t use nicer cloths and napkins because we perceive it as work to get them ready for an event and then to get them clean after said event.</p>",1,"2022-11-25 00:00:00.0000000","3ff3b26f-21d1-4518-88f2-6310d8fedc11"},
                    { "a14f2b7b-9df6-4729-9406-4200f5c112ea","Impacts Of External Factors On Wearing Products","blog3.png","<p>In the process of using garment products, it is affected by many external factors. Let&#39;s study together with Sapy about the effects of the external environment to have a laundry process for each product!</p><ul><li>&nbsp;Impact of dirt - Dirt clinging to the fabric not only reduces the beauty of the product, but also reduces hygiene and destroys the fabric. Dust, sand, dirt particles often have sharp edges, they rub off, abrasive fibers. - If from the outside environment there are grease substances that penetrate into the fabric, it will create conditions to keep dirt particles tight and the longer it stays, the higher the degree of destruction of the fabric. - Perspiration for a long time will also attach dirt to the fabric, causing the fabric to have an unpleasant smell, discoloring, making the product darker and older. - Salts, acids, sugars and proteins often make fabrics stiff and reduce frictional resistance, and they create a growth environment for molds and bacteria, leading to fabric destruction. Destruction depends on the concentration and duration of the contamination on the fabric.</li><li><p>&nbsp;Effect of friction This is the mechanical action that causes a material to wear out due to rubbing against another harder material or the same material as they slide over each other. - With clothing occurs a lot in the collar, elbows, thighs, knees, cuffs, and buttocks. At these points, the fabric is often shiny, frayed, and the fabric becomes thinner</p></li><li><p>&nbsp;Impact of light and use environment When using a garment, the fabric will be slowly changed under the influence of light (photochemical reaction) and environmental conditions.</p></li><li><p>&nbsp;Impact of microorganisms and insects - In the process of using garments that are easily destroyed by microorganisms. - Microorganisms from the air and from water - Some materials contain groups that promote the growth of microorganisms such as cotton, wool, fur, etc. - In addition to microorganisms, there are insects such as cockroaches, insects that hide and lay eggs to destroy the product.</p></li></ul>",1,"2022-11-25 00:00:00.0000000","3ff3b26f-21d1-4518-88f2-6310d8fedc11"},
            });

            // Insert data orders
            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "orderid", "fullname", "emailaddress", "contactnumber", "address", "price", "note", "paymentstatus", "orderstatus", "createdat", "updatedat", "userid", "paymentid", "couponid" },
                values: new object[,]{
                    { "4439e146-1318-43e4-8d25-78c8ecd51367","nguyen viet anh","terminatornva@gmail.com","0394880635","phuc tho - ha noi" ,2.0,"hi",1,2,"2022-10-25 00:00:00.0000000","2022-10-25 00:00:00.0000000",null,"0709196d-913e-45a7-9fd3-564d3737e9c7",null},
                    { "cdbab9e6-c1ff-420f-82c2-0fa6bccc6de3","nguyen vu phong","terminatornva@gmail.com","0394880635","phuc tho - ha noi" ,2.0,"hi",1,1,"2022-11-25 00:00:00.0000000","2022-10-25 00:00:00.0000000",null,"0709196d-913e-45a7-9fd3-564d3737e9c7",null},
                    { "c637a20e-9366-4e67-99d0-ce84cee12ec3","dang thi bich ngoc","ngoc1233456789@gmail.com","0973461932","soc son - ha noi" ,23.0,"hi",1,0,"2022-11-25 00:00:00.0000000","2022-10-25 00:00:00.0000000","97576b8c-2f9c-452b-b259-bd49174fdb71","0709196d-913e-45a7-9fd3-564d3737e9c7","e4e1d0cd-f636-4567-bfb2-40b244c504e5"},
                    { "d6cb9131-e295-437c-8a12-611dc0a9a216","Nguyen Vu Phong","nguyenvuphong2202@gmail.com","097384635","ha noi" ,15.0,"hi",1,0,"2022-09-25 00:00:00.0000000","2022-10-25 00:00:00.0000000","97576b8c-2f9c-452b-b259-bd49174fdb71","0709196d-913e-45a7-9fd3-564d3737e9c7",null}
                 });

            // Insert data Order Items
            migrationBuilder.InsertData(
            table: "OrderItems",
            columns: new[] { "OrderItemId", "PriceUnit", "Quantity", "OrderId", "UnitId" },
            values: new object[,]{
                    { 1,2.0,1,"cdbab9e6-c1ff-420f-82c2-0fa6bccc6de3","0717d50d-5572-4073-a032-27ee306767b7"},
                    { 2,2.0,1,"4439e146-1318-43e4-8d25-78c8ecd51367","0717d50d-5572-4073-a032-27ee306767b7"},
                    { 3,5.0,2,"c637a20e-9366-4e67-99d0-ce84cee12ec3","3b3c0552-9721-48bd-bba0-99aaee3f74b0"},
                    { 4,15.0,1,"c637a20e-9366-4e67-99d0-ce84cee12ec3","a43b41fe-6d63-4553-9f22-48c810b8ce2f"},
                    { 5,15.0,3,"d6cb9131-e295-437c-8a12-611dc0a9a216","3b3c0552-9721-48bd-bba0-99aaee3f74b0"}
             });

            //  Insert data Used Coupons
            migrationBuilder.InsertData(
            table: "UsedCoupons",
            columns: new[] { "UsedCouponId", "UserId", "CouponId" },
            values: new object[,]{
                    { 1,"97576b8c-2f9c-452b-b259-bd49174fdb71","e4e1d0cd-f636-4567-bfb2-40b244c504e5"}
            });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "UsedCoupons");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Services");


        }
    }
}
