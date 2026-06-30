using System;
using System.Collections.Generic;
using System.Linq;
using GameHub.Infrastructure.Entity;

namespace GameHub.Infrastructure.Data;

public static class DbInitializer
{
    public static void Seed(GameHubDbContext context)
    {
        // Перевіряємо, чи база вже заповнена. Якщо так — нічого не робимо
        if (context.Games.Any()) return;

        // 1. Додаємо Жанри
        var genres = new Dictionary<string, Genre>
        {
            { "Action", new Genre { Name = "Action-Adventure" } },
            { "RPG", new Genre { Name = "RPG" } },
            { "Shooter", new Genre { Name = "Shooter" } },
            { "Horror", new Genre { Name = "Survival Horror" } }
        };
        context.Genres.AddRange(genres.Values);

        // 2. Додаємо Розробників
        var devRockstar = new Developer { Name = "Rockstar Games", Description = "Легендарна американська студія, відома своїми безпрецедентними світами, деталізацією та кінематографічним підходом до екшенів у відкритому світі." };
        var devUbisoft = new Developer { Name = "Ubisoft", Description = "Французький гігант ігрової індустрії, творець величезних історичних та пригодницьких франшиз з акцентом на дослідження масштабних локацій." };
        var devSony = new Developer { Name = "PlayStation Studios", Description = "Об'єднання внутрішніх студій Sony (Santa Monica, Naughty Dog, Insomniac), які створюють шедевральні сюжетні ексклюзиви з глибокою історією." };
        var devWarhorse = new Developer { Name = "Warhorse Studios", Description = "Чеська студія, яка зробила переворот у жанрі RPG, зробивши ставку на абсолютну історичну достовірність, реалізм Середньовіччя та складну бойову систему." };
        var devCapcom = new Developer { Name = "Capcom", Description = "Японський ігровий титан, лідер у жанрі survival horror та екшенів, який подарував світові культові всесвіти з унікальним геймплеєм." };
        var devRocksteady = new Developer { Name = "Rocksteady Studios", Description = "Британська студія, яка встановила еталон супергеройських ігор у світі, створивши похмуру та революційну трилогію про Бетмена." };

        context.Developers.AddRange(devRockstar, devUbisoft, devSony, devWarhorse, devCapcom, devRocksteady);
        context.SaveChanges(); // Зберігаємо, щоб отримати Id для зв'язків

        var gamesToAdd = new List<Game>();

        // --- ROCKSTAR GAMES ---
        AddGameWithImage(gamesToAdd, devRockstar.Id, genres["Action"].Id, "Grand Theft Auto III", 
            "Революційна гра, яка перевела серію у 3D та започаткувала сучасну епоху екшенів у відкритому світі. Історія німого злочинця Клода у похмурому Ліберті-Сіті.", 
            "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1546970/header.jpg");

        AddGameWithImage(gamesToAdd, devRockstar.Id, genres["Action"].Id, "Grand Theft Auto: Vice City", 
            "Культова атмосфера Майамі 1980-х років, неонові вогні, яскравий стиль та незабутня музика. Томмі Версетті підкорює кримінальний світ узбережжя.", 
            "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1546990/header.jpg");

        AddGameWithImage(gamesToAdd, devRockstar.Id, genres["Action"].Id, "Grand Theft Auto: San Andreas", 
            "Гігантський штат, три величезні міста та історія Сі-Джея, який повертається додому в Лос-Сантос. Легендарна свобода вибору та RPG-елементи у відкритому світі.", 
            "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1547000/header.jpg");

        AddGameWithImage(gamesToAdd, devRockstar.Id, genres["Action"].Id, "Grand Theft Auto IV", 
            "Похмура та неймовірно реалістична історія емігранта Ніко Белліка у пошуках американської мрії. Гра вражає своєю фізикою та деталізованим живим містом.", 
            "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1221030/header.jpg");

        AddGameWithImage(gamesToAdd, devRockstar.Id, genres["Action"].Id, "Grand Theft Auto V", 
            "Один із найуспішніших медіапроєктів в історії. Сюжет розгортається навколо трьох абсолютно різних грабіжників у сонячному Лос-Сантосі.", 
            "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/271590/header.jpg");

        AddGameWithImage(gamesToAdd, devRockstar.Id, genres["Action"].Id, "Red Dead Redemption", 
            "Епічна історія заходу епохи Дикого Заходу. Колишній бандит Джон Марстон змушений полювати на своїх старих друзів, щоб врятувати власну родину.", 
            "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2668510/header.jpg");

        AddGameWithImage(gamesToAdd, devRockstar.Id, genres["Action"].Id, "Red Dead Redemption 2", 
            "Еталон ігор із відкритим світом та глибоким сюжетом. Історія Артура Моргана та банди Датча ван дер Лінде у неймовірно живому та трагічному світі серця Америки.", 
            "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1174180/header.jpg");

        AddGameWithImage(gamesToAdd, devRockstar.Id, genres["Shooter"].Id, "Max Payne 3", 
            "Кінематографічний нуарний шутер з унікальною механікою уповільнення часу Bullet Time. Постарілий та зломлений Макс Пейн працює охоронцем у небезпечному Сан-Паулу.", 
            "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/204100/header.jpg");


        // --- UBISOFT ---
        string ubiCover = "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1515530/header.jpg"; // Заглушка AC
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Assassin's Creed", "Початок легендарного протистояння Ассасінів та Тамплієрів. Пригоди Альтаїра на Святій Землі під час Хрестового походу.", ubiCover);
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Assassin's Creed II", "Епоха Відродження в Італії та історія становлення Еціо Аудіторе. Одна з найкращих ігор серії.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/33230/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Assassin's Creed: Brotherhood", "Продовження історії Еціо, де він очолює римське Братство та бореться проти корумпованого сімейства Борджіа.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/48190/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Assassin's Creed: Revelations", "Фінал пригод Еціо в Константинополі, де він іде слідами свого великого предка Альтаїра.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/201870/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Assassin's Creed III", "Війна за незалежність США крізь призму долі індіанця-напівкровки Коннора Кенуея.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/911400/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Assassin's Creed IV: Black Flag", "Золота доба піратства в Карибському морі. Едвард Кенуей шукає багатства і потрапляє в таємну війну орденів.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/242050/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Assassin's Creed: Rogue", "Унікальний сюжет, де головний герой розчаровується в ассасінах і стає мисливцем на них на боці тамплієрів.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/311560/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Assassin's Creed: Unity", "Французька революція у розкішному, масштабно відтвореному Парижі з революційним паркуром та кооперативом.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/289650/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Assassin's Creed: Syndicate", "Промислова революція в Лондоні 19 століття. Історія близнюків Джейкоба та Іві Фрай, які керують вуличними бандами.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/368500/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["RPG"].Id, "Assassin's Creed: Origins", "Перезапуск франшизи в Древньому Єгипті з повноцінною RPG системою та історією створення Братства.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/582160/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["RPG"].Id, "Assassin's Creed: Odyssey", "Масштабна подорож Стародавньою Грецією під час Пелопоннеської війни з вибором спартанського героя.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/812140/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["RPG"].Id, "Assassin's Creed: Valhalla", "Епічні завоювання вікінгів у ранньосередньовічній Англії, будівництво поселення та набіги.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2208920/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Assassin's Creed: Mirage", "Повернення до коріння серії: класичний стелс та паркур на колоритних вулицях Багдада 9 століття.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2333900/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Assassin's Creed: Shadows", "Довгоочікувана подорож до феодальної Японії. Сюжет закручений навколо двох різних героїв: ніндзя та самурая.", "https://gamedev.com/covers/shadows.jpg"); // Тимчасовий лінк
        
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Far Cry 3", "Божевільна та дика пригода на тропічному острові, яка подарувала нам незабутнього антагоніста Вааса Монтенегро.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/220240/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Far Cry 4", "Виживання у засніжених та небезпечних Гімалаях проти харизматичного диктатора Пейгана Міна.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/298110/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Far Cry 5", "Протистояння релігійному культу «Брама Едему» на чолі з Йосипом Сідом у глибинці американського штату Монтана.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/552520/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Far Cry 6", "Сучасна партизанська війна на тропічному острові Яра, що застряг у часі під гнітом диктатора Антона Кастійо.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1966720/header.jpg");

        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Watch Dogs", "Екшен про хакера Ейдена Пірса в Чикаго, де вся міська інфраструктура контролюється єдиною комп'ютерною мережею.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/243470/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Watch Dogs 2", "Яскравий та молодіжний сиквел у Сан-Франциско, де група хакерів DedSec бореться проти корпоративного нагляду.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/447040/header.jpg");
        AddGameWithImage(gamesToAdd, devUbisoft.Id, genres["Action"].Id, "Watch Dogs: Legion", "Антиутопічний Лондон майбутнього, де ви можете завербувати та взяти під контроль абсолютно будь-якого жителя міста.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2231380/header.jpg");


        // --- SONY (PLAYSTATION STUDIOS) ---
        AddGameWithImage(gamesToAdd, devSony.Id, genres["Action"].Id, "God of War (2018)", "Монументальне скандинавське переосмислення історії Кратоса. Складний шлях батька та сина у суворому світі міфів.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1593500/header.jpg");
        AddGameWithImage(gamesToAdd, devSony.Id, genres["Action"].Id, "God of War: Ragnarok", "Фінал скандинавської дилогії. Наближається кінець світу, і Кратос з Атреєм мають обрати свій шлях у війні богів.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2322010/header.jpg");
        AddGameWithImage(gamesToAdd, devSony.Id, genres["Action"].Id, "Marvel's Spider-Man Remastered", "Неймовірна супергеройська гра з видовищними польотами на павутині по Нью-Йорку та емоційним сюжетом Пітера Паркера.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1817070/header.jpg");
        AddGameWithImage(gamesToAdd, devSony.Id, genres["Action"].Id, "Marvel's Spider-Man: Miles Morales", "Історія становлення юного Майлза Моралеса, який освоює унікальні біоелектричні сили та захищає свій дім Гарлем.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1817130/header.jpg");
        AddGameWithImage(gamesToAdd, devSony.Id, genres["Action"].Id, "Marvel's Spider-Man 2", "Спільна пригода Пітера та Майлза. Герої борються з Крейвеном-Мисливцем та культовим інопланетним Симбіотом-Веномом.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2651260/header.jpg");
        AddGameWithImage(gamesToAdd, devSony.Id, genres["Action"].Id, "Uncharted 4: A Thief's End", "Кінематографічний фінал пригод шукача скарбів Натана Дрейка. Неймовірні краєвиди, піратські таємниці та сімейна драма.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1659420/header.jpg");
        AddGameWithImage(gamesToAdd, devSony.Id, genres["Horror"].Id, "The Last of Us Part I", "Культова постапокаліптична подорож контрабандиста Джоела та дівчинки Еллі через зруйновані грибком США.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1888930/header.jpg");
        AddGameWithImage(gamesToAdd, devSony.Id, genres["Action"].Id, "The Last of Us Part II", "Жорстока, емоційна та технічно бездоганна драма про помсту, ненависть, прощення та циклічність насильства.", "https://sony.com/covers/tlou2.jpg");
        AddGameWithImage(gamesToAdd, devSony.Id, genres["Action"].Id, "Days Gone", "Подорож байкера Дікона Сент-Джона на мотоциклі через мальовничий Орегон, переповнений гігантськими ордами зомбі-фріків.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1259420/header.jpg");


        // --- WARHORSE STUDIOS ---
        AddGameWithImage(gamesToAdd, devWarhorse.Id, genres["RPG"].Id, "Kingdom Come: Deliverance", "Реалістична середньовічна RPG без магії та драконів. Шлях простого сина коваля Індржиха у часи громадянської війни в Богемії.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/379430/header.jpg");
        AddGameWithImage(gamesToAdd, devWarhorse.Id, genres["RPG"].Id, "Kingdom Come: Deliverance II", "Масштабне продовження історії Індро. Сюжет стає епічнішим, міста більшими, а бойова система — ще витонченішою.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1771300/header.jpg");


        // --- CAPCOM ---
        AddGameWithImage(gamesToAdd, devCapcom.Id, genres["Horror"].Id, "Resident Evil 2 (Remake)", "Еталонний переосмислений хорор. Виживання Леона Кеннеді та Клер Редфілд у зараженому поліцейському відділку Раккун-Сіті.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/883710/header.jpg");
        AddGameWithImage(gamesToAdd, devCapcom.Id, genres["Horror"].Id, "Resident Evil 3 (Remake)", "Динамічний хорор-екшен про спробу Джил Валентайн втекти з палаючого міста, поки її переслідує невблаганний Немезис.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/952060/header.jpg");
        AddGameWithImage(gamesToAdd, devCapcom.Id, genres["Horror"].Id, "Resident Evil 4 (Remake)", "Шедевральне оновлення культової гри. Агент Леон Кеннеді рятує доньку президента з моторошного іспанського селища культистів.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2050650/header.jpg");
        AddGameWithImage(gamesToAdd, devCapcom.Id, genres["Horror"].Id, "Resident Evil 5", "Кооперативний екшен-хорор в Африці, де Кріс Редфілд та Шева Аломар борються з новим біоорганічним вірусом під палючим сонцем.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/21690/header.jpg");
        AddGameWithImage(gamesToAdd, devCapcom.Id, genres["Horror"].Id, "Resident Evil 6", "Грандіозне переплетення чотирьох різних сюжетних кампаній по всьому світу з упором на масштабні голлівудські екшен-сцени.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/221040/header.jpg");
        AddGameWithImage(gamesToAdd, devCapcom.Id, genres["Horror"].Id, "Resident Evil 7: Biohazard", "Повернення до витоків із видом від першої особи. Жахлива історія Ітана Вінтерса у гнилому маєтку шаленої сімейки Бейкерів.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/418370/header.jpg");
        AddGameWithImage(gamesToAdd, devCapcom.Id, genres["Horror"].Id, "Resident Evil Village", "Продовження історії Ітана в загадковому засніженому європейському селищі серед вампірів, перевертнів та леді Дімітреску.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1196590/header.jpg");
        AddGameWithImage(gamesToAdd, devCapcom.Id, genres["Horror"].Id, "Resident Evil 9", "Нова таємнича глава легендарного хорор-всесвіту з новими загрозами та знайомими героями серії.", "https://capcom.com/covers/re9.jpg");


        // --- ROCKSTEADY ---
        AddGameWithImage(gamesToAdd, devRocksteady.Id, genres["Action"].Id, "Batman: Arkham Asylum", "Похмура атмосфера психіатричної лікарні Аркгем, де Бетмен опиняється замкненим сам на сам із Джокером та всіма психами.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/35140/header.jpg");
        AddGameWithImage(gamesToAdd, devRocksteady.Id, genres["Action"].Id, "Batman: Arkham City", "Масштабне розширення гри у відкритому секторі Готема, перетвореному на гігантську в'язницю для найнебезпечніших бандитів.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/200260/header.jpg");
        AddGameWithImage(gamesToAdd, devRocksteady.Id, genres["Action"].Id, "Batman: Arkham Origins", "Приквел історії, що показує молодого і брутального Бетмена, на якого оголосили полювання вісім смертоносних вбивць напередодні Різдва.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/209000/header.jpg");
        AddGameWithImage(gamesToAdd, devRocksteady.Id, genres["Action"].Id, "Batman: Arkham Knight", "Епічний фінал серії з можливістю керувати технологічним Бетмобілем. Протистояння Опудалу та загадковому Лицарю Аркгему.", "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/208650/header.jpg");

        context.Games.AddRange(gamesToAdd);
        context.SaveChanges();
    }

    private static void AddGameWithImage(List<Game> list, int devId, int genreId, string title, string desc, string imgUrl)
    {
        var game = new Game
        {
            Title = title,
            Description = desc,
            DeveloperId = devId,
            GenreId = genreId,
            Images = new List<GameImage> { new GameImage { ImagePath = imgUrl } }
        };
        list.Add(game);
    }
}